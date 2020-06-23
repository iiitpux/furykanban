using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuryKanban.Client.Core
{
	public class LoaderService
	{
		private int _loaderCount = 0;
		private object _lock = new object();

		public EventHandler OnStart;
		public EventHandler OnEnd;

		public async Task LoadStart()
		{
			lock (_lock)
			{
				_loaderCount++;
				OnStart?.Invoke(this, null);
			}
		}

		public async Task LoadEnd()
		{
			lock (_lock)
			{
				_loaderCount--;
				if (_loaderCount == 0)
					OnEnd?.Invoke(this, null);
			}
		}
	}
}
