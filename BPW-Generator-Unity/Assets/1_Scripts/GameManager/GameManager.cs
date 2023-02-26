using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float PhysicsFrameRate; //UPS
    private float physicsTimer;

    private List<BaseClass> baseClassList;
    private List<BaseClassLate> baseClassLateList;

    private void Awake()
    {
        baseClassList = new List<BaseClass>();
        baseClassLateList = new List<BaseClassLate>();

        baseClassList.AddRange(FindObjectsOfType<BaseClass>());
        baseClassLateList.AddRange(FindObjectsOfType<BaseClassLate>());

        for (int i = 0; i < baseClassList.Count; i++) { baseClassList[i].OnAwake(); }
        for (int i = 0; i < baseClassLateList.Count; i++) { baseClassLateList[i].OnAwake(); }
    }

    private void Start()
    {
        for (int i = 0; i < baseClassList.Count; i++) { baseClassList[i].OnStart(); }
        for (int i = 0; i < baseClassLateList.Count; i++) { baseClassLateList[i].OnStart(); }
    }

    private void Update()
    {
        //Calc PhysicsUpdate()
        if (physicsTimer > 1 / PhysicsFrameRate)
        {
            PhysicsUpdate();
            physicsTimer = 0;
        }
        physicsTimer += Time.deltaTime;

        for (int i = 0; i < baseClassList.Count; i++) { baseClassList[i].OnUpdate(); }
        for (int i = 0; i < baseClassLateList.Count; i++) { baseClassLateList[i].OnUpdate(); }
    }

    private void PhysicsUpdate()
    {
        for (int i = 0; i < baseClassList.Count; i++) { baseClassList[i].OnPhysicsUpdate(); }
        for (int i = 0; i < baseClassLateList.Count; i++) { baseClassLateList[i].OnPhysicsUpdate(); }
    }

    public void AddObject(BaseClass _object)
    {
        _object.OnAwake();
        _object.OnStart();
        baseClassList.Add(_object);
    }

    public void RemoveObject(BaseClass _object)
    {
        baseClassList.Remove(_object);
        Destroy(_object.gameObject);
    }
}