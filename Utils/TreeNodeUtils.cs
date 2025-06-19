using System.Windows.Forms;

namespace mapper_refactor.Utils;

public static class TreeNodeUtils
{
    public static TreeNode? FindNodeByObject(TreeNodeCollection nodes, object targetObject)
    {
        foreach (TreeNode node in nodes)
        {
            if (node is BoundTreeNode boundNode && boundNode.BoundObject == targetObject)
            {
                return node;
            }

            var result = FindNodeByObject(node.Nodes, targetObject);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public static void CollectExpandedNodes(TreeNodeCollection nodes, List<string> expandedPaths)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.IsExpanded)
            {
                expandedPaths.Add(node.FullPath);
            }

            if (node.Nodes.Count > 0)
            {
                CollectExpandedNodes(node.Nodes, expandedPaths);
            }
        }
    }

    public static void RestoreExpandedNodes(TreeNodeCollection nodes, IEnumerable<string> expandedPaths)
    {
        foreach (var path in expandedPaths)
        {
            var node = FindNodeByPath(nodes, path);
            node?.Expand();
        }
    }

    private static TreeNode? FindNodeByPath(TreeNodeCollection nodes, string path)
    {
        var pathParts = path.Split('\\');
        var currentNodes = nodes;
        TreeNode? currentNode = null;

        foreach (var part in pathParts)
        {
            currentNode = null;
            foreach (TreeNode node in currentNodes)
            {
                if (node.Text == part)
                {
                    currentNode = node;
                    currentNodes = node.Nodes;
                    break;
                }
            }

            if (currentNode == null)
            {
                return null;
            }
        }

        return currentNode;
    }
}

public class BoundTreeNode : TreeNode
{
    private object? _boundObject;
    private ContextMenuStrip? _contextMenu;

    public object? BoundObject
    {
        get => _boundObject;
        set
        {
            _boundObject = value;
            Text = value?.ToString() ?? string.Empty;
        }
    }

    public ContextMenuStrip? NodeContextMenu
    {
        get => _contextMenu;
        set
        {
            _contextMenu = value;
            ContextMenuStrip = value;
        }
    }

    public BoundTreeNode() : base() { }

    public BoundTreeNode(string text) : base(text) { }

    public BoundTreeNode(string text, int imageIndex, int selectedImageIndex)
        : base(text, imageIndex, selectedImageIndex) { }

    public BoundTreeNode(string text, object boundObject)
        : base(text)
    {
        BoundObject = boundObject;
    }
} 