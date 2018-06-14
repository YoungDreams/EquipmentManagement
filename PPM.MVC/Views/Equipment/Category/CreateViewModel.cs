using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using PPM.Commands;
using PPM.Entities;

namespace PPM.MVC.Views.Equipment.Category
{
    public class CreateViewModel : CreateEquipmentCategoryCommand
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
        public EquipmentCategoryTreeView ProductCategoryTreeView { get; set; }
    }

    public class EquipmentCategoryTreeView
    {
        private readonly List<EquipmentCategory> _productCategories;

        public EquipmentCategoryTreeView(List<EquipmentCategory> productCategories = null, bool lastLayerOnly = false)
        {
            _productCategories = productCategories;
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
                treeBuilder.Append("<li data-jstree=\"{disabled:" + (disabled ? "true" : "false") + "}\">" +
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


        public string GetCategoryText(int categoryId, List<EquipmentCategory> purchaseProductCategories)
        {
            return purchaseProductCategories.SingleOrDefault(x => x.Id == categoryId)?.Name;
        }

        public EquipmentCategoryTreeView GetProductCategoryTreeView()
        {
            var categoryTreeView = new EquipmentCategoryTreeView
            {
                Trees = new List<TreeNode>(),
            };

            if (!_productCategories.Any())
            {
                return categoryTreeView;
            }

            var minLayer = _productCategories.Min(x => x.Layer);
            var maxLayer = _productCategories.Min(x => x.Layer);
            var layerLength = _productCategories.Select(x => x.Layer).Distinct().Count();
            var rootLayerTrees = RetrieveRootTreeNodesByParentId(minLayer, _productCategories);

            //if (!LastLayerOnly)
            //{
            //    categoryTreeView.Trees.Add(new TreeNode
            //    {
            //        HasSubTreeNodes = false,
            //        Text = "全部",
            //        Value = "",
            //    });
            //}
            foreach (var category in rootLayerTrees)
            {
                category.SubTreeNodes = new List<TreeNode>();
                category.SubTreeNodes = RetrieveSubTreeNodes(int.Parse(category.Value), _productCategories);
                categoryTreeView.Trees.Add(category);
            }

            return categoryTreeView;
        }

        public EquipmentCategoryTreeView GetProductCategoryTreeView(List<EquipmentCategory> purchaseProductCategories)
        {
            var categoryTreeView = new EquipmentCategoryTreeView
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
            //if (!LastLayerOnly)
            //{
            //    categoryTreeView.Trees.Add(new TreeNode
            //    {
            //        HasSubTreeNodes = false,
            //        Text = "全部",
            //        Value = "",
            //    });
            //}
            foreach (var category in rootLayerTrees)
            {
                category.SubTreeNodes = new List<TreeNode>();
                category.SubTreeNodes = RetrieveSubTreeNodes(int.Parse(category.Value), purchaseProductCategories);
                categoryTreeView.Trees.Add(category);
            }

            return categoryTreeView;
        }

        private List<TreeNode> RetrieveRootTreeNodesByParentId(int layer, List<EquipmentCategory> purchaseProductCategories)
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

        private List<TreeNode> RetrieveSubTreeNodes(int parentId, List<EquipmentCategory> purchaseProductCategories)
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
}