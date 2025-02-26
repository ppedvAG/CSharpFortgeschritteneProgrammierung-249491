using System.Text.Json.Serialization;

namespace Lab5_SolarExport.WithIds.Data
{
    public class Node<T>
    {
        private Guid _id;

        private T _item;

        private Guid _parentId = default!;

        private List<Guid> _children = new List<Guid>();

        public Node()
        {

        }
        public Node(T item)
        {
            Item = item;
            Id = Guid.NewGuid();
        }

        public Node(T item, Guid parentId)
            : this(item)
        {
            ParentId = parentId;
        }

        public Node(Node<T> node)
        {
            Id = node.Id;
            ParentId = node.ParentId;
            Item = node.Item;
            Children = node.Children;
        }


        [JsonPropertyName("Id")]
        public Guid Id { get => _id; set => _id = value; }

        [JsonPropertyName("Item")]
        public T Item { get => _item; set => _item = value; }

        [JsonPropertyName("ParentId")]
        public Guid ParentId { get => _parentId; set => _parentId = value; }

        [JsonPropertyName("Children")]
        public List<Guid> Children { get => _children; set => _children = value; }

        public void AddChild(Node<T> childNode)
        {
            childNode.ParentId = Id;
            Children.Add(childNode.Id);
        }
    }
}
