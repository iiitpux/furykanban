using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuryKanban.Client.Core
{
	public class DragndropService
	{
		public EventHandler OnDrop { set; get; }

		public EventHandler OnShowDropzone { set; get; }
		public EventHandler OnHideDropzone { set; get; }
		public int DragId { set; get; }
		public int DropId { set; get; }
		public int ParentId { set; get; }

		public void Drag(int id)
		{
			DragId = id;
		}

		public void Drop(int id)
		{
			DropId = id;
		}

		public async Task ShowDropzoneAsync()
		{
			OnShowDropzone.Invoke(this, null);
		}

		public async Task HideDropzoneAsync()
		{
			OnHideDropzone.Invoke(this, null);
		}

		public void ParentDrop(int id)
		{
			ParentId = id;
			OnDrop?.Invoke(this, null);
		}
	}
}
