using Cysharp.Threading.Tasks;
using System;

public interface ILoadingOperation 
{
    UniTask Load();
}
