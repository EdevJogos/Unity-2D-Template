using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Range
{
    /// <summary>
    /// Returns a random value between min and max.
    /// </summary>
    public float Value { get { return Random.Range(min, max); } }

    public float min;
    public float max;

    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

public class RandomNoRepeat
{
    private IEnumerable<int> _range;
    private List<int> _indexes = new List<int>();

    /// <summary>
    /// Do not construct this object outside of a method scope.
    /// </summary>
    public RandomNoRepeat(IEnumerable<int> p_range)
    {
        _range = p_range;
        _indexes.Clear();
        _indexes.AddRange(_range);
    }

    /// <summary>
    /// Returns a random number without reapeting previous returned ones.
    /// </summary>
    public int GetRandom()
    {
        int __radomIndex = Random.Range(0, _indexes.Count - 1);
        int __chosenNumber = _indexes[__radomIndex];

        _indexes.RemoveAt(__radomIndex);

        if(_indexes.Count == 0)
        {
            _indexes.AddRange(_range);
        }

        return __chosenNumber;
    }

    /// <summary>
    /// Resets the previous choosen values, allowing them to be selected again.
    /// </summary>
    public void Restart()
    {
        _indexes.Clear();
        _indexes.AddRange(_range);
    }
}
