using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IService <T>
{

    IEnumerator GetRandom(System.Action <T> callback);

    IEnumerator Add(T t, System.Action <T> callback);

}