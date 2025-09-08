using System.Security.Cryptography.X509Certificates;


public class DLL<T> : IEnumerable<T>, IList<T>
{

    public class DNode
    {
        public DNode? prev;
        public T? value;
        public DNode? next;
        public DNode(DNode? prev, T? value, DNode next)
        {
            this.prev = prev;
            this.value = value;
            this.next = next;
        }


    }
    public DNode? head;
    public DNode? tail;
    public DLL()
    {
        this.head = new DNode(null, default, null);
        this.tail = new DNode(this.head, default, null);
        this.head = new DNode(null, default, this.tail);
    }

}