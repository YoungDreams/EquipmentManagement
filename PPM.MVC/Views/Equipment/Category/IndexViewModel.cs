using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using PPM.Commands;
using PPM.Entities;
using PPM.Query;
using PPM.Web.Common;

namespace PPM.MVC.Views.Equipment.Category
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public EquipmentCategoryQuery Query { get; set; }
        public IEnumerable<EquipmentCategory> Items { get; set; }
        public FancyTreeNodeView FancyTreeNodeView { get; set; }
        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "EquipmentCategory"),
                Command = new DeleteEquipmentCategoryCommand { Id = id, ReturnUrl = strUrl }
            };
        }
    }

    public class FancyTreeNodeView
    {
        public FancyTreeNodeView(List<EquipmentCategory> purchaseProductCategories)
        {
            Trees = GetProductCategoryTreeView(purchaseProductCategories);
        }
        public List<FancyTreeNode> Trees { get; set; }

        public List<FancyTreeNode> GetProductCategoryTreeView(List<EquipmentCategory> categories)
        {
            var trees = new List<FancyTreeNode>();

            if (!categories.Any())
            {
                return trees;
            }
            var minLayer = categories.Min(x => x.Layer);
            var rootLayerTrees = RetrieveRootTreeNodesByParentId(minLayer, categories);

            foreach (var category in rootLayerTrees)
            {
                category.children = new List<FancyTreeNode>();
                category.children = RetrieveSubTreeNodes(int.Parse(category.key), categories);
                trees.Add(category);
            }

            return trees;
        }

        private List<FancyTreeNode> RetrieveRootTreeNodesByParentId(int layer, List<EquipmentCategory> purchaseProductCategories)
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

        private List<FancyTreeNode> RetrieveSubTreeNodes(int parentId, List<EquipmentCategory> purchaseProductCategories)
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