namespace Lab_1_Doubly_Linked_Lists;
/*
 * ====================================================================
 * DLL (Doubly Linked List) Unit Tests
 * ====================================================================
 * 
 * Test Author: Claude (Anthropic AI Assistant)
 * Created: September 8, 2025
 * 
 * Description:
 * Comprehensive unit test suite for the DLL<T> (Doubly Linked List) 
 * implementation using xUnit framework. Tests cover node creation, 
 * list initialization, insertion operations, and deletion operations.
 * 
 * ====================================================================
 */
public class UnitTest1
{
    [Fact]
    public void DNode_Constructor_Tests()
    {
        var node = new DLL<int>.DNode(null, 42, null);
        Assert.Equal(42, node.value);

    }
    [Fact]
    public void DLL_Constructor_ShouldCreateEmptyList()
    {
        var dll = new DLL<int>();
        Assert.NotNull(dll.head);
        Assert.Equal(0, dll.head.value);

    }

    [Fact]
    public void Add_SingleElement_ShouldAddToList()
    {
        var dll = new DLL<int>();
        dll.Insert(dll.tail, 42);
        Assert.Equal(42, dll.tail.prev.value);
        Assert.Equal(42, dll.tail.prev.prev.next.value);
        Assert.Equal(0, dll.tail.prev.prev.value);
        Assert.Equal(42, dll.head.next.value);

    }
    [Fact]
    public void Remove_DeletesNodeFromList()
    {
        //Arrange
        var dll = new DLL<int>();
        dll.Insert(dll.tail, 42);
        //Act
        dll.Remove(dll.head.next);
        //Assert
        Assert.Equal(dll.tail, dll.head.next);
    }
}

