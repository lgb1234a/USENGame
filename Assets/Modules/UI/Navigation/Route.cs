// Created by LunarEclipse on 2024-6-19 6:25.

using System.Threading.Tasks;

namespace Luna.UI.Navigation
{
    public class Route<T>
    {
        public Task<T> Task => _tcs.Task;
        private TaskCompletionSource<T> _tcs = new();
    }
}