namespace PROJ.DSA;

public class UnionFind<T> where T : notnull
{
    private readonly Dictionary<T, int> _index = new();
    private readonly List<int> _parent = new();

    public void Add(T elem)
    {
        if (_index.ContainsKey(elem))
            return;

        int id = _parent.Count;
        _index[elem] = id;
        _parent.Add(id);
    }

    private int FindIndex(int x)
    {
        if (_parent[x] != x)
            _parent[x] = FindIndex(_parent[x]);

        return _parent[x];
    }

    public int Find(T elem)
    {
        if (!_index.TryGetValue(elem, out int id))
            return -1;

        return FindIndex(id);
    }

    public bool Union(T e1, T e2)
    {
        if (!_index.TryGetValue(e1, out int id1) || !_index.TryGetValue(e2, out int id2))
            return false;

        int root1 = FindIndex(id1);
        int root2 = FindIndex(id2);

        if (root1 == root2)
            return false;

        _parent[root2] = root1;
        return true;
    }
}