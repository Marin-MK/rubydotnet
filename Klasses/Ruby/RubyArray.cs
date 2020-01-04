using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyArray : RubyObject
    {
        public new static Class CreateClass(IntPtr Pointer)
        {
            Class c = new Class("Array", Pointer);
            return c;
        }

        public RubyArray(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public RubyArray(int Capacity)
        {
            this.Pointer = Internal.rb_ary_new();
            for (int i = 0; i < Capacity; i++)
                Internal.rb_ary_store(this.Pointer, i, Internal.QNil);
        }

        public RubyArray()
        {
            this.Pointer = Internal.rb_ary_new();
        }

        public delegate void Iterator(RubyObject Object);
        public delegate void IndexIterator(RubyObject Object, int Index);
        public delegate object ReturningIterator(RubyObject Object);
        public delegate object ReturningIndexIterator(RubyObject Object, int Index);

        public RubyObject this[int Index]
        {
            get
            {
                if (Index >= Length)
                    throw new Exception("Index out of bounds.");
                return new RubyObject(Internal.rb_ary_entry(this.Pointer, Index));
            }
            set
            {
                if (Index >= Length)
                {
                    int nils = Index - Length;
                    for (int i = 0; i < nils; i++) Internal.rb_ary_push(this.Pointer, Internal.QNil);
                }
                Internal.rb_ary_store(this.Pointer, Index, value.Pointer);
            }
        }

        public int Length
        {
            get
            {
                int v = (int) Internal.NUM2LONG(Internal.rb_funcallv(this.Pointer, Internal.rb_intern("length"), 0));
                return v;
            }
        }

        public void RemoveAt(int Index)
        {
            Internal.rb_ary_delete_at(this.Pointer, Index);
        }

        public void Add(RubyObject Object)
        {
            Internal.rb_ary_push(this.Pointer, Object.Pointer);
        }

        public void Each(Iterator iterator)
        {
            for (int i = 0; i < Length; i++)
            {
                iterator(new RubyObject(Internal.rb_ary_entry(this.Pointer, i)));
            }
        }

        public void EachWithIndex(IndexIterator iterator)
        {
            for (int i = 0; i < Length; i++)
            {
                iterator(new RubyObject(Internal.rb_ary_entry(this.Pointer, i)), i);
            }
        }

        public void Map(ReturningIterator iterator)
        {
            for (int i = 0; i < Length; i++)
            {
                object o = iterator(new RubyObject(Internal.rb_ary_entry(this.Pointer, i)));
                if (!(o is RubyObject) && !(o is IntPtr)) throw new Exception("Invalid return type in Array.Map");
                IntPtr ptr = o is RubyObject ? (o as RubyObject).Pointer : (IntPtr)o;
                Internal.rb_ary_store(this.Pointer, i, ptr);
            }
        }

        public void MapWithIndex(ReturningIndexIterator iterator)
        {
            for (int i = 0; i < Length; i++)
            {
                object o = iterator(new RubyObject(Internal.rb_ary_entry(this.Pointer, i)), i);
                if (!(o is RubyObject) && !(o is IntPtr)) throw new Exception("Invalid return type in Array.Map");
                IntPtr ptr = o is RubyObject ? (o as RubyObject).Pointer : (IntPtr)o;
                Internal.rb_ary_store(this.Pointer, i, ptr);
            }
        }

        public void Insert(int Index, RubyObject Object)
        {
            Internal.rb_funcallv(this.Pointer, Internal.rb_intern("insert"), 2, Internal.LONG2NUM(Index), Object.Pointer);
        }

        public override string ToString()
        {
            return "[... (size " + Length.ToString() + ")]";
        }
    }
}
