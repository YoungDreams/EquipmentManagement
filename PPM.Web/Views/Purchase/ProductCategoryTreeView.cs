using System.Collections.Generic;
using System.Text;
using PensionInsurance.Entities;
using System.Linq;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;

namespace PensionInsurance.Web.Views.Purchase
{
    public class ProductCategoryTreeView
    {
        private List<PurchaseProductCategory> _purchaseProductCategories;

        public ProductCategoryTreeView(List<PurchaseProductCategory> purchaseProductCategories = null, bool lastLayerOnly = false)
        {
            _purchaseProductCategories = purchaseProductCategories;
            LastLayerOnly = lastLayerOnly;
        }
        public List<TreeNode> Trees { get; set; }
        public bool LastLayerOnly { get; set; }
        //public string CategoryText => categories.SingleOrDefault(x => x.Id == product.ProductCategory.Id).Name
        public string CategoryText { get; set; }
        public int CategoryId { get; set; }

        public string GetSubTreeNodes(List<TreeNode> treeNodes)
        {
            var treeBuilder = new StringBuilder();
            treeBuilder.Append("<ul>");
            foreach (var tree in treeNodes)
            {
                var lastLayer = GetLastLayer(tree);
                var disabled = (LastLayerOnly && tree.Layer != lastLayer);
                treeBuilder.Append("<li data-jstree=\"{disabled:"+ (disabled ? "true" : "false") + "}\">" +
                                   $"<a class=\"{ (disabled ? "jstree-disabled" : "")}\" href='javascript:; ' data-id=\"{tree.Value}\" data-text=\"{tree.Text}\"> {tree.Text} </a>");
                if (tree.HasSubTreeNodes)
                {
                    treeBuilder.Append(GetSubTreeNodes(tree.SubTreeNodes));
                }
                treeBuilder.Append("</li>");
            }
            treeBuilder.Append("</ul>");
            return treeBuilder.ToString();
        }

        public string GetSubCheckAbleTreeNodes(List<TreeNode> treeNodes)
        {
            var treeBuilder = new StringBuilder();
            treeBuilder.Append("<ul class=\"jstree-children\">");
            foreach (var tree in treeNodes)
            {
                var lastLayer = GetLastLayer(tree);
                var disabled = (LastLayerOnly && tree.Layer != lastLayer);
                treeBuilder.Append("<li data-jstree=\"{disabled:" + (disabled ? "true" : "false") + "}\">" +
                                   $"<a class=\"{ (disabled ? "jstree-anchor" : "jstree-anchor jstree-clicked")}\" href='javascript:; ' data-id=\"{tree.Value}\" data-text=\"{tree.Text}\"> " +
                                   $"<i class=\"jstree-icon jstree-checkbox\" role=\"presentation\"></i>" +
                                   $"{tree.Text} " +
                                   $"</a>");
                if (tree.HasSubTreeNodes)
                {
                    treeBuilder.Append(GetSubTreeNodes(tree.SubTreeNodes));
                }
                treeBuilder.Append("</li>");
            }
            treeBuilder.Append("</ul>");
            return treeBuilder.ToString();
        }

        private int GetLastLayer(TreeNode treeNode)
        {
            if (treeNode.HasSubTreeNodes)
            {
                return GetLastLayer(treeNode.SubTreeNodes.First());
            }
            return treeNode.Layer;
        }


        public string GetCategoryText(int categoryId, List<PurchaseProductCategory> purchaseProductCategories)
        {
            return purchaseProductCategories.SingleOrDefault(x => x.Id == categoryId)?.Name;
        }

        public ProductCategoryTreeView GetProductCategoryTreeView()
        {
            var categoryTreeView = new ProductCategoryTreeView
            {
                Trees = new List<TreeNode>(),
            };

            if (!_purchaseProductCategories.Any())
            {
                return categoryTreeView;
            }

            var minLayer = _purchaseProductCategories.Min(x => x.Layer);
            var maxLayer = _purchaseProductCategories.Min(x => x.Layer);
            var layerLength = _purchaseProductCategories.Select(x => x.Layer).Distinct().Count();
            var rootLayerTrees = RetrieveRootTreeNodesByParentId(minLayer, _purchaseProductCategories);
            
            if (!LastLayerOnly)
            {
                categoryTreeView.Trees.Add(new TreeNode
                {
                    HasSubTreeNodes = false,
                    Text = "全部",
                    Value = "",
                });
            }
            foreach (var category in rootLayerTrees)
            {
                category.SubTreeNodes = new List<TreeNode>();
                category.SubTreeNodes = RetrieveSubTreeNodes(int.Parse(category.Value), _purchaseProductCategories);
                categoryTreeView.Trees.Add(category);
            }

            return categoryTreeView;
        }

        public ProductCategoryTreeView GetProductCategoryTreeView(List<PurchaseProductCategory> purchaseProductCategories)
        {
            var categoryTreeView = new ProductCategoryTreeView
            {
                Trees = new List<TreeNode>()
            };

            if (!purchaseProductCategories.Any())
            {
                return categoryTreeView;
            }
            var minLayer = purchaseProductCategories.Min(x => x.Layer);
            var layerLength = purchaseProductCategories.Select(x => x.Layer).Distinct().Count();
            var rootLayerTrees = RetrieveRootTreeNodesByParentId(minLayer, purchaseProductCategories);
            if (!LastLayerOnly)
            {
                categoryTreeView.Trees.Add(new TreeNode
                {
                    HasSubTreeNodes = false,
                    Text = "全部",
                    Value = "",
                });
            }
            foreach (var category in rootLayerTrees)
            {
                category.SubTreeNodes = new List<TreeNode>();
                category.SubTreeNodes = RetrieveSubTreeNodes(int.Parse(category.Value), purchaseProductCategories);
                categoryTreeView.Trees.Add(category);
            }

            return categoryTreeView;
        }

        private List<TreeNode> RetrieveRootTreeNodesByParentId(int layer, List<PurchaseProductCategory> purchaseProductCategories)
        {
            var subNodes = purchaseProductCategories.Where(x => x.Layer == layer).Select(x => new TreeNode
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                ParentId = x.ParentId.Value,
                Layer = x.Layer,
                HasSubTreeNodes = purchaseProductCategories.Any(z => z.ParentId == x.Id),
            }).ToList();
            return subNodes;
        }

        private List<TreeNode> RetrieveSubTreeNodes(int parentId, List<PurchaseProductCategory> purchaseProductCategories)
        {
            var subNodes = purchaseProductCategories.Where(x => x.ParentId == parentId).Select(x => new TreeNode
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Layer = x.Layer,
                HasSubTreeNodes = purchaseProductCategories.Any(z => z.ParentId == x.Id),
                SubTreeNodes = purchaseProductCategories.Any(z => z.ParentId == x.Id) ? RetrieveSubTreeNodes(x.Id, purchaseProductCategories) : new List<TreeNode>()
            }).ToList();
            return subNodes;
        }
    }
    public class TreeNode : SelectListItem
    {
        public bool IsRoot { get; set; }
        public bool HasSubTreeNodes { get; set; }
        public int ParentId { get; set; }
        public int Layer { get; set; }
        public List<TreeNode> SubTreeNodes { get; set; }
    }

    public class FancyTreeNodeView
    {
        public FancyTreeNodeView(List<PurchaseProductCategory> purchaseProductCategories)
        {
            Trees = GetProductCategoryTreeView(purchaseProductCategories);
        }
        public List<FancyTreeNode> Trees { get; set; }

        public List<FancyTreeNode> GetProductCategoryTreeView(List<PurchaseProductCategory> purchaseProductCategories)
        {
            var trees = new List<FancyTreeNode>();

            if (!purchaseProductCategories.Any())
            {
                return trees;
            }
            var minLayer = purchaseProductCategories.Min(x => x.Layer);
            var rootLayerTrees = RetrieveRootTreeNodesByParentId(minLayer, purchaseProductCategories);

            foreach (var category in rootLayerTrees)
            {
                category.children = new List<FancyTreeNode>();
                category.children = RetrieveSubTreeNodes(int.Parse(category.key), purchaseProductCategories);
                trees.Add(category);
            }

            return trees;
        }

        private List<FancyTreeNode> RetrieveRootTreeNodesByParentId(int layer, List<PurchaseProductCategory> purchaseProductCategories)
        {
            var subNodes = purchaseProductCategories.Where(x => x.Layer == layer).Select(x => new FancyTreeNode
            {
                title = x.Name,
                key = x.Id.ToString(),
                ParentId = x.ParentId.Value,
                Layer = x.Layer,
                HasSubTreeNodes = purchaseProductCategories.Any(z => z.ParentId == x.Id),
            }).ToList();
            return subNodes;
        }

        private List<FancyTreeNode> RetrieveSubTreeNodes(int parentId, List<PurchaseProductCategory> purchaseProductCategories)
        {
            var subNodes = purchaseProductCategories.Where(x => x.ParentId == parentId).Select(x => new FancyTreeNode
            {
                title = x.Name,
                key = x.Id.ToString(),
                ParentId = x.ParentId.Value,
                Layer = x.Layer,
                HasSubTreeNodes = purchaseProductCategories.Any(z => z.ParentId == x.Id),
                children = purchaseProductCategories.Any(z => z.ParentId == x.Id) ? RetrieveSubTreeNodes(x.Id, purchaseProductCategories) : new List<FancyTreeNode>()
            }).ToList();
            return subNodes;
        }
    }

    public class FancyTreeNode
    {
        public string title { get; set; }
        public bool folder => true;
        public string key { get; set; }
        public int ParentId { get; set; }
        public int Layer { get; set; }
        public bool HasSubTreeNodes { get; set; }
        public List<FancyTreeNode> children { get; set; }
    }
}