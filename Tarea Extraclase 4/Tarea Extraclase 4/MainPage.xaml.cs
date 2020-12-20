 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tarea_Extraclase_4
{
    public partial class MainPage : ContentPage
    {
        AATree<int, int> tree = new AATree<int, int>();
        public MainPage()
        {
            InitializeComponent();
           
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var valor = double.Parse(Insertar.Text);
            ;        
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {

        }
    }

    public class AATree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class Node
        {
            // node internal data
            internal int level;
            internal Node left;
            internal Node right;

            // user data
            internal TKey key;
            internal TValue value;

            // constuctor for the sentinel node
            internal Node()
            {
                this.level = 0;
                this.left = this;
                this.right = this;
            }

            // constuctor for regular nodes (that all start life as leaf nodes)
            internal Node(TKey key, TValue value, Node sentinel)
            {
                this.level = 1;
                this.left = sentinel;
                this.right = sentinel;
                this.key = key;
                this.value = value;
            }
        }
        Node root;
        Node sentinel;
        Node deleted;

        public AATree()
        {
            root = sentinel = new Node();
            deleted = null;
        }

        private void Skew(ref Node node)
        {
            if (node.level == node.left.level)
            {
                // rotate right
                Node left = node.left;
                node.left = left.right;
                left.right = node;
                node = left;
            }
        }

        private void Split(ref Node node)
        {
            if (node.right.right.level == node.level)
            {
                // rotate left
                Node right = node.right;
                node.right = right.left;
                right.left = node;
                node = right;
                node.level++;
            }
        }

        private bool Insert(ref Node node, TKey key, TValue value)
        {
            if (node == sentinel)
            {
                node = new Node(key, value, sentinel);
                return true;
            }

            int compare = key.CompareTo(node.key);
            if (compare < 0)
            {
                if (!Insert(ref node.left, key, value))
                    return false;
            }
            else if (compare > 0)
            {
                if (!Insert(ref node.right, key, value))
                    return false;
            }
            else
            {
                return false;
            }

            Skew(ref node);
            Split(ref node);

            return true;
        }

        private bool Delete(ref Node node, TKey key)
        {
            if (node == sentinel)
            {
                return (deleted != null);
            }

            int compare = key.CompareTo(node.key);
            if (compare < 0)
            {
                if (!Delete(ref node.left, key))
                    return false;
            }
            else
            {
                if (compare == 0)
                    deleted = node;
                if (!Delete(ref node.right, key))
                    return false;
            }

            if (deleted != null)
            {
                deleted.key = node.key;
                deleted.value = node.value;
                deleted = null;
                node = node.right;
            }
            else if (node.left.level < node.level - 1
                  || node.right.level < node.level - 1)
            {
                --node.level;
                if (node.right.level > node.level)
                    node.right.level = node.level;
                Skew(ref node);
                Skew(ref node.right);
                Skew(ref node.right.right);
                Split(ref node);
                Split(ref node.right);
            }

            return true;
        }
    }
}
