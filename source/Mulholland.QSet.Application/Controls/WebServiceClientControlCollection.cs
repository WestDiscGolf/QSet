using System;
using System.Collections;
using System.Collections.Specialized;

namespace Mulholland.QSet.Application.Controls
{
	/// <summary>
	/// WebServiceClientControl Collection.
	/// </summary>
	internal class WebServiceClientControlCollection : NameObjectCollectionBase
	{
		private event WebServiceClientControlCollection.ItemAddedEvent _itemAdded;
		private event WebServiceClientControlCollection.ItemRemovedEvent _itemRemoved;

		/// <summary>
		/// Constructs an empty WebServiceClientControlCollection.
		/// </summary>
		public WebServiceClientControlCollection() {}

		#region events

		/// <summary>
		/// Delegate for ItemAdded event.
		/// </summary>
		public delegate void ItemAddedEvent(object sender, ItemMovedEventArgs e);		


		/// <summary>
		/// Delegate for ItemRemoved event.
		/// </summary>
		public delegate void ItemRemovedEvent(object sender, ItemMovedEventArgs e);


		/// <summary>
		/// Event arguments for ItemAdded or ItemRemoved events.
		/// </summary>
		public class ItemMovedEventArgs : EventArgs
		{
			private WebServiceClientControl _item;

			/// <summary>
			/// Constructs the object with the minum requirements.
			/// </summary>
			/// <param name="item">Item that was added or removed.</param>
			public ItemMovedEventArgs(WebServiceClientControl item)
			{
				_item = item;
			}


			/// <summary>
			/// Item that was added or removed.
			/// </summary>
			public WebServiceClientControl Item
			{
				get
				{
					return _item;
				}
			}
		}


		/// <summary>
		/// Raised when an item is added to the collection.
		/// </summary>
		public event WebServiceClientControlCollection.ItemAddedEvent ItemAdded
		{
			add
			{
				_itemAdded += value;
			}
			remove
			{
				_itemAdded -= value;
			}
		}


		/// <summary>
		/// Raised when an item is removed from the collection.
		/// </summary>
		public event WebServiceClientControlCollection.ItemRemovedEvent ItemRemoved
		{
			add
			{
				_itemRemoved += value;
			}
			remove
			{
				_itemRemoved -= value;
			}
		}


		/// <summary>
		/// Raises the ItemAdded event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		private void OnItemAdded(ItemMovedEventArgs e)
		{
			try
			{
				_itemAdded(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the ItemRemoved event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		private void OnItemRemoved(ItemMovedEventArgs e)
		{
			try
			{
				_itemRemoved(this, e);
			}
			catch {}
		}

		#endregion

		#region enumeration support
		
		/// <summary>
		/// Provides foreach enumeration support.
		/// </summary>
		/// <returns></returns>
		public new IEnumerator GetEnumerator()
		{			
			return new Enumerator(this);
		}
		
		
		private class Enumerator : IEnumerator
		{		
			private WebServiceClientControlCollection _col;
			private int _index;

			public Enumerator(WebServiceClientControlCollection col)
			{
				_col = col;
				_index = -1;
			}

			public void Reset()
			{
				_index = -1;
			}

			public object Current
			{
				get
				{					
					return _col[_index];
				}
			}

			public bool MoveNext()
			{
				_index ++;
				if (_index < _col.Count)
					return true;
				else
					return false;
			}					
		}		

		#endregion

		/// <summary>
		/// Constructs a WebServiceClientControlCollection, populating with the provided collection.
		/// </summary>
		/// <param name="WebServiceClientControlCollection">Collection to build constructed collection from.</param>
		public WebServiceClientControlCollection(WebServiceClientControlCollection WebServiceClientControlCollection)
		{
			for(int i = 0; i < WebServiceClientControlCollection.Count - 1; i ++)
			{
				string key = WebServiceClientControlCollection.BaseGetKey(i);
				base.BaseAdd(key, WebServiceClientControlCollection[i]);
			}
		}


		/// <summary>
		/// Adds a WebServiceClientControl to the collection.
		/// </summary>
		/// <param name="WebServiceClientControl">WebServiceClientControl to add.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if any arguments are set to null.</exception>
		public void Add(string key, WebServiceClientControl WebServiceClientControl)
		{
			if (key == null)
				throw new ArgumentNullException("key");
			if (WebServiceClientControl == null)
				throw new ArgumentNullException("WebServiceClientControl");
			
			if (this.Exists(key))
				throw new ArgumentOutOfRangeException("key", "Key already exists in collection.");
					
			base.BaseAdd(key, WebServiceClientControl);

			OnItemAdded(new WebServiceClientControlCollection.ItemMovedEventArgs(WebServiceClientControl));
		}


		/// <summary>
		/// Removes the WebServiceClientControl with the specified key.
		/// </summary>
		/// <param name="key">Key of WebServiceClientControl to remove.</param>
		public void Remove(string key)
		{
			WebServiceClientControl WebServiceClientControl = null;

			lock (this)
			{
				WebServiceClientControl = (WebServiceClientControl)base.BaseGet(key);
				base.BaseRemove(key);
			}

			if (WebServiceClientControl != null)
				OnItemRemoved(new WebServiceClientControlCollection.ItemMovedEventArgs(WebServiceClientControl));
		}


		/// <summary>
		/// Removes the WebServiceClientControl at the specified index.
		/// </summary>
		/// <param name="index">Index to remove at.</param>
		public void RemoveAt(int index)
		{
			WebServiceClientControl WebServiceClientControl = null;

			lock (this)
			{
				WebServiceClientControl = (WebServiceClientControl)base.BaseGet(index);
				base.BaseRemoveAt(index);
			}

			if (WebServiceClientControl != null)
				OnItemRemoved(new WebServiceClientControlCollection.ItemMovedEventArgs(WebServiceClientControl));
		}

		
		/// <summary>
		/// Gets or sets the Message at/with the specified key.
		/// </summary>
		public WebServiceClientControl this[string key]
		{
			get
			{
				return (WebServiceClientControl)base.BaseGet(key);
			}
			set
			{
				base.BaseSet(key, value);
			}
		}


		/// <summary>
		/// Gets or sets the Message at/with the specified index.
		/// </summary>
		public WebServiceClientControl this[int index]
		{
			get
			{
				return (WebServiceClientControl)base.BaseGet(index);
			}
			set
			{
				base.BaseSet(index, value);				
			}
		}	


		/// <summary>
		/// Checks to see if a particular key exists witin the collection.
		/// </summary>
		/// <param name="key">Key to search for.</param>
		/// <returns>true if the key exists, else false.</returns>
		public bool Exists(string key)
		{
			bool result = false;

			if (base.BaseGet(key) != null)
				result = true;

			return result;
		}

	}

}
