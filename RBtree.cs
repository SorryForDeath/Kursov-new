using Microsoft.VisualBasic;
using System;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace RedBlackTree
{
    public enum Color
    {
        Black,
        Red
    }
    public struct Dev
    {
        public string Country;
        public string Name;
        public int AllSails;
        public Dev(string country, string name, int allsails)
        {
            Country = country;
            Name = name;
            AllSails = allsails; 
        }
        public void Print()
        {
            Console.Write(Name + " from " + Country + ", total sales: " + AllSails.ToString());
        }
        public void Set(string country, string name, int allsails)
        {
            Country = country;
            Name = name;
            AllSails = allsails;
        }
    }
    public class Node
    {
        public Node Left;
        public Node Right;
        public Node Parent;
        public Dev Dev;
        public Color Color;

        public Node()
        {}
        public Node(Node left, Node right, Node parent, Dev dev, Color color)
        {
            Left = left;
            Right = right;
            Parent = parent;
            Dev = dev;
            Color = color;
        }
    }
    public class Tree
    {
        Node head = null;

        private Color GetColor(Node node)
        {
            return node == null ? Color.Black : node.Color;
        }
        private void Paint(Node node, Color color)
        {
            if (node != null)
                node.Color = color;
        }
        public bool Search(Dev dev)
        {
            if (!IsEmpty())
            {
                Node element = head;
                while (element != null)
                {
                    if (IsFirstDateEquivalent(dev, element))
                        return true;
                    else
                    {
                        if (IsFirstDateBigger(dev, element))
                            element = element.Right;
                        else element = element.Left;
                    }
                }
                return false;
            }
            else return false;
        }
        public bool IsEmpty()
        {
            return head == null;
        }
        private bool IsFirstDateBigger(Node first, Node node)
        {
              if (first.Dev.AllSails > node.Dev.AllSails)
                return true;
            else return false;
        }
        private bool IsFirstDateBigger(Dev first, Node node)
        {
            if (first.AllSails > node.Dev.AllSails)
                return true;
            else return false;
        }
        private bool IsFirstDateEquivalent(Dev dev, Node node)
        {
            return (dev.AllSails == node.Dev.AllSails);
        }
        public Dev Maximum() //Поиск максимального элемента
        {
            if (!IsEmpty())
                return SubTreeMaximum(head).Dev;
            else
            {
               Dev dev = new(" ", " ", 0);
               return dev;
            }
        }
        private Node SubTreeMaximum(Node element)
        {
            while (element.Right != null)
                element = element.Right;
            return element;
        }
        public Dev Miminum() //Поиск минимального элемента
        {
            if (head != null)
                return SubTreeMinimum(head).Dev;
            else
            {
                Dev dev = new(" ", " ", 0);
                return dev;
            }
        }
        private Node SubTreeMinimum(Node element)
        {
            while (element.Left != null)
                element = element.Left;
            return element;
        }
        public void Add(Dev dev) //Добавление элемента в дерево
        {
            if (!Search(dev))
            {
                Node newElement = new() { Dev = dev };
                AddInTree(newElement);
            }
        }
        private void AddInTree(Node element)
        {
            Node prePointer = null;
            Node pointer = head;
            while ((pointer != null))
            {
                prePointer = pointer;
                if (IsFirstDateBigger(pointer, element))
                    pointer = pointer.Left;
                else
                    pointer = pointer.Right;
            }
            element.Parent = prePointer;
            if (prePointer == null)
            {
                head = element;
                head.Parent = null;
            }
            else if (IsFirstDateBigger(prePointer, element))
                prePointer.Left = element;
            else
                prePointer.Right = element;
            element.Left = null;
            element.Right = null;
            Paint(element, Color.Red);
            AddFixUp(element);
        }
        private void AddFixUp(Node element)
        {
            while (GetColor(element.Parent) == Color.Red)
            {
                bool IsElementLeft = (element.Parent == element.Parent.Parent.Left);
                element = AddFixUpFork(element, IsElementLeft);
            }
            Paint(head, Color.Black);
        }
        private Node AddFixUpFork(Node element, bool IsBrotherRight)
        {
            Node brother;
            brother = IsBrotherRight ? element.Parent.Parent.Right : element.Parent.Parent.Left;
            if (GetColor(brother) == Color.Red)
                element = AddFixUpCaseOne(element, brother);
            else
            {
                if (IsBrotherRight)
                {
                    if (element == element.Parent.Right)
                        element = AddFixUpCaseTwo(element, IsBrotherRight);
                }
                else
                {
                    if (element == element.Parent.Left)
                        element = AddFixUpCaseTwo(element, IsBrotherRight);
                }
                AddFixUpCaseThree(element, IsBrotherRight);
            }
            return element;
        }
        private Node AddFixUpCaseOne(Node element, Node brother)
        {
            Paint(element.Parent, Color.Black);
            Paint(brother, Color.Black);
            Paint(element.Parent.Parent, Color.Red); //Тут есть возможность для ошибки
            return element.Parent.Parent;
        }
        private Node AddFixUpCaseTwo(Node element, bool IsBrotherRight)
        {
            element = element.Parent;
            if (IsBrotherRight)
                RotateLeft(element);
            else
                RotateRight(element);
            return element;
        }
        private void AddFixUpCaseThree(Node element, bool IsBrotherRight)
        {
            Paint(element.Parent, Color.Black);
            Paint(element.Parent.Parent, Color.Red);
            if (IsBrotherRight)
                RotateRight(element.Parent.Parent);
            else
                RotateLeft(element.Parent.Parent);
        }
        public void Delete(Dev dev) //Удаление элемента из дерева
        {
            Node NodeToDelete = GetExistingNode(dev);
            if (NodeToDelete != null)
                DeleteFromTree(NodeToDelete);
        }
        private Node GetExistingNode(Dev dev)
        {
            Node element = head;
            while (element != null)
            {
                if (IsFirstDateEquivalent(dev, element))
                    return element;
                else
                {
                    if (IsFirstDateBigger(dev, element))
                        element = element.Right;
                    else element = element.Left;
                }
            }
            return null;
        }
        private void DeleteFromTree(Node element)
        {
            Color originalColor = GetColor(element);
            if (element.Left != null && element.Right != null)
            {
                Node elementForSwap = SubTreeMinimum(element.Right);
                originalColor = GetColor(elementForSwap);
                Transplant(element, elementForSwap);
                elementForSwap.Color = GetColor(element);
            }
            element.Color = Color.Black;
            if (originalColor == Color.Black)
                DeleteFixUp(element);
            if (element != head)
            {
                if (IsLeftChild(element))
                    element.Parent.Left = null;
                else
                    element.Parent.Right = null;
            }
            else
            {
                if (element.Right != null)
                    head = element.Right;
                else
                    head = element.Left;
            }
            Paint(head, Color.Black);
        }
        private void DeleteFixUp(Node element)
        {
            while (element != head && GetColor(element) == Color.Black)
            {
                bool IsBrotherRight = (element == element.Parent.Left);
                element = DeleteFixUpFork(element, IsBrotherRight);
            }
            Paint(element, Color.Black);
        }
        private Node DeleteFixUpFork(Node element, bool IsBrotherRight)
        {
            Node brother;
            brother = IsBrotherRight ? element.Parent.Right : element.Parent.Left;
            if (GetColor(brother) == Color.Red)
                brother = DeleteFixUpCaseOne(element, brother, IsBrotherRight);

            if (GetColor(brother.Left) == Color.Black && GetColor(brother.Right) == Color.Black)
                element = DeleteFixUpCaseTwo(element, brother);
            else
            {
                if (IsBrotherRight)
                {
                    if (GetColor(brother.Right) == Color.Black)
                        brother = DeleteFixUpCaseThree(element, brother, IsBrotherRight);

                }
                else
                {
                    if (GetColor(brother.Left) == Color.Black)
                        brother = DeleteFixUpCaseThree(element, brother, IsBrotherRight);
                }
                element = DeleteFixUpCaseFour(element, brother, IsBrotherRight);
            }
            return element;
        }
        private Node DeleteFixUpCaseOne(Node element, Node brother, bool IsBrotherRight)
        {
            if (IsBrotherRight)
            {
                Paint(brother, Color.Black);
                Paint(element.Parent, Color.Red);
                RotateLeft(element.Parent);
                return element.Parent.Right;//brother
            }
            else
            {
                Paint(brother, Color.Black);
                Paint(element.Parent, Color.Red);
                RotateRight(element.Parent);
                return element.Parent.Left;
            }
        }
        private Node DeleteFixUpCaseTwo(Node element, Node brother)
        {
            Paint(brother, Color.Red);
            return element.Parent;
        }
        private Node DeleteFixUpCaseThree(Node element, Node brother, bool IsBrotherRight)
        {
            if (IsBrotherRight)
            {
                Paint(brother.Left, Color.Black);
                Paint(brother, Color.Red);
                RotateRight(brother);
                return element.Parent.Right;
            }
            else
            {
                Paint(brother.Right, Color.Black);
                Paint(brother, Color.Red);
                RotateLeft(brother);
                return element.Parent.Left;
            }
        }
        private Node DeleteFixUpCaseFour(Node element, Node brother, bool IsElementLeft)
        {
            if (IsElementLeft)
            {
                Paint(brother, GetColor(element.Parent));
                Paint(element.Parent, Color.Black);
                Paint(brother.Right, Color.Black);
                RotateLeft(element.Parent);
                return head;
            }
            else
            {
                Paint(brother, GetColor(element.Parent));
                Paint(element.Parent, Color.Black);
                Paint(brother.Left, Color.Black);
                RotateRight(element.Parent);
                return head;
            }
        }
        private void Transplant(Node first, Node second)
        {
            if (head == first || head == second)
                head = (first == head) ? second : first;

            Node buffer = new();
            AssignNewPosition(buffer, first);
            if (InCloseProximity(first, second))
            {
                if (buffer.Parent == second)
                {
                    buffer.Parent = first;
                    if (IsLeftChild(first))
                        second.Left = second;
                    else
                        second.Right = second;
                }
                else
                {

                    if (IsLeftChild(second))
                        buffer.Left = first;
                    else
                        buffer.Right = first;
                    second.Parent = second;
                }
            }
            AssignNewPosition(first, second);
            AssignNewPosition(second, buffer);
            ExternalLinksRepair(first, second);
            ExternalLinksRepair(second, first);
        }
        private void AssignNewPosition(Node NodeToAssign, Node NodeWithPosition)
        {
            NodeToAssign.Parent = NodeWithPosition.Parent;
            NodeToAssign.Left = NodeWithPosition.Left;
            NodeToAssign.Right = NodeWithPosition.Right;
        }
        private bool InCloseProximity(Node first, Node second)
        {
            return (first.Parent == second || second.Parent == first);
        }
        private void ExternalLinksRepair(Node node, Node previousChild)
        {
            if (node.Left != null)
                node.Left.Parent = node;
            if (node.Right != null)
                node.Right.Parent = node;
            if (node != head)
            {
                if (node.Parent.Left == previousChild)
                    node.Parent.Left = node;
                else if (node.Parent.Right == previousChild)
                    node.Parent.Right = node;
            }
        }
        private bool IsLeftChild(Node element)
        {
            if (element.Parent != null)
                return !(element == element.Parent.Right);
            else
                return false;
        }
        private void RotateLeft(Node element)
        {
            Node rightChild = element.Right;
            element.Right = rightChild.Left;
            if (rightChild.Left != null)
                rightChild.Left.Parent = element;

            rightChild.Parent = element.Parent;
            if (element.Parent == null)
                head = rightChild;

            else if (element == element.Parent.Left)
                element.Parent.Left = rightChild;
            else
                element.Parent.Right = rightChild;
            rightChild.Left = element;
            element.Parent = rightChild;
        }
        private void RotateRight(Node element)
        {
            Node LeftChild = element.Left;
            element.Left = LeftChild.Right;
            if (LeftChild.Right != null)
                LeftChild.Right.Parent = element;

            LeftChild.Parent = element.Parent;
            if (element.Parent == null)
                head = LeftChild;

            else if (element == element.Parent.Right)
                element.Parent.Right = LeftChild;
            else
                element.Parent.Left = LeftChild;
            LeftChild.Right = element;
            element.Parent = LeftChild;
        }
        public void InOrder() //Прямой обход
        {
            InOrderWalk(head);
            Console.WriteLine();
        }
        private void InOrderWalk(Node element)
        {
            if (element != null)
            {
                InOrderWalk(element.Left);
                element.Dev.Print();
                Console.Write(' ');
                InOrderWalk(element.Right);
            }
        }
        public void InOrderInverted() //Прямой инвертированный обход
        {
            InOrderInvertedWalk(head);
            Console.WriteLine();
        }
        private void InOrderInvertedWalk(Node element)
        {
            if (element != null)
            {
                InOrderInvertedWalk(element.Right);
                element.Dev.Print();
                Console.Write(' ');
                InOrderInvertedWalk(element.Left);
            }
        }
        public void PreOrder() //Обратный обход
        {
            PreOrderWalk(head);
            Console.WriteLine();
        }
        private void PreOrderWalk(Node element)
        {
            if (element != null)
            {
                element.Dev.Print();
                Console.Write(' ');
                PreOrderWalk(element.Left);
                PreOrderWalk(element.Right);
            }
        }
        public void PostOrder() //Симметричный обход
        {
            PostOrderWalk(head);
            Console.WriteLine();
        }
        private void PostOrderWalk(Node element)
        {
            if (element != null)
            {
                PostOrderWalk(element.Left);
                PostOrderWalk(element.Right);
                element.Dev.Print();
                Console.Write(' ');
            }
        }
        public void PrintTree() //Вывод дерева на консоль
        {
            if (!IsEmpty())
            {
                Console.WriteLine("———————————————————————");
                PrintTreeInternal(head, 5);
                Console.WriteLine("———————————————————————");
            }
            else
                Console.WriteLine("Tree is empty!");
        }
        private void PrintTreeInternal(Node element, int offset)
        {
            if (element != null)
            {
                PrintTreeInternal(element.Right, offset + 5);
                for (int i = 1; i < offset; i++)
                    Console.Write(" ");
                if (GetColor(element) == Color.Red)
                    Console.ForegroundColor = ConsoleColor.Red;

                element.Dev.Print();
                Console.WriteLine();
                Console.ResetColor();
                PrintTreeInternal(element.Left, offset + 5);
            }
        }
        public void Clear()//Очистка дерева
        {
            ClearInternal(head);
        }
        private void ClearInternal(Node element)
        {
            if (element != null)
            {
                ClearInternal(element.Left);
                ClearInternal(element.Right);
                if (element != head)
                {
                    if (IsLeftChild(element))
                        element.Parent.Left = null;
                    else
                        element.Parent.Right = null;
                }
                else
                {
                    head = null;
                }

            }
        }
    }
}
