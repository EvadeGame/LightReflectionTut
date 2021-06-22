using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    // Laser 本体
    public LineRenderer laser;
    //存储 Laser 经过的路径的列表
    public List<Vector3> laserPoint = new List<Vector3>();
    void Update(){
        // 使 Laser Gun 以一定都速度旋转
        transform.Rotate(Vector3.forward*10f*Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            laser.gameObject.SetActive(true);
            CastLaser();
            laser.positionCount = laserPoint.Count;
            laser.SetPositions(laserPoint.ToArray());
        }
        else
        {
            laser.gameObject.SetActive(false);
        }
    }

    void CastLaser()
    {
        // 原来是忘记了清空 laserPoint, 如果用递归实现，那么此时可能已经栈溢出了
        laserPoint.Clear();
        // 从 Laser Gun 的位置出发
        var startPoint = transform.position;
        // 发射方向
        var direction = transform.right;
        // 有了第一个出发点
        laserPoint.Add(startPoint);

        int i = 0;
        do
        {
            var hit = Physics2D.Raycast(startPoint, direction);
            // 添加射线击中点到路径中
            laserPoint.Add(hit.point);
            
            // 新的发射方向, 借助 Unity 内置的 Reflect 函数，可以通过 入射向量（inDirection) 和
            // 法向量 hit.normal 得到反射后的向量 direction
            // 但是 startPoint 已经改变了，所以先求方向
            
            // 可以看到，中间的点位置都一样
            direction = Vector2.Reflect(hit.point - (Vector2)startPoint, hit.normal);
            
            // 将下一次发射起点设定为击中点
            startPoint = (Vector3)hit.point + direction*0.01f;

            i++;
        } while (i < 10);
    }
}
