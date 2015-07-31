using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ScriptableObjectDatabase<T> : ScriptableObject where T : class
{
    //[SerializeField]
    List<T> db = new List<T>();

    public int Count
    {
        get { return db.Count; }
    }

    public void Add(T item)
    {
        db.Add(item);
        SetEditorDirty();
    }

    public void Insert(int index, T item)
    {
        db.Insert(index, item);
        SetEditorDirty();
    }

    public void Remove(T item)
    {
        db.Remove(item);
        SetEditorDirty();
    }

    public void Remove(int index)
    {
        db.RemoveAt(index);
        SetEditorDirty();
    }

    public T Get(int index)
    {
        return db.ElementAt(index);
    }

    public void Replace(int index, T item)
    {
        db[index] = item;
        SetEditorDirty();
    }

    private void SetEditorDirty()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

#if UNITY_EDITOR
    public static U GetDatabase<U>(string dbPath, string dbName) where U : ScriptableObject
    {
    //    var dbFullPath = string.Format("Assets/{0}/{1}", dbPath, dbName);

    //    var db = AssetDatabase.LoadAssetAtPath<U>(dbFullPath);

    //    if (db == null)
    //    {
    //        if (!AssetDatabase.IsValidFolder("Assets/" + dbPath))
    //            AssetDatabase.CreateFolder("Assets", dbPath);

    //        db = ScriptableObject.CreateInstance<U>();

    //        AssetDatabase.CreateAsset(db, dbFullPath);
    //        AssetDatabase.SaveAssets();
    //        AssetDatabase.Refresh();
    //    }

    //    return db;
        return null;
    }
#endif
}
