using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    private bool m_Lock;
    private bool m_IsSpawning;
    private float m_SpawnDelay;
   
    private WaveStandard m_Wave;

    public void Init(WaveStandard wave, float spawnDelay)
    {
        m_SpawnDelay = spawnDelay;
        m_Wave = wave;
        m_Lock = false;
        m_IsSpawning = false;
    }

 
    public void ToggleSpawning(bool toggle)
    {
        m_IsSpawning = toggle;
        if (toggle)
        {

            if (!m_Lock)
            {
                m_Lock = true;
                StartCoroutine(ie_AttemptToSpawn());
            }
 
            m_Lock = true;
 
        }
        else
        {
            StopCoroutine(ie_AttemptToSpawn());
            m_Lock = false;
        }
    }

    private IEnumerator ie_AttemptToSpawn()
    {
        yield return new WaitForSeconds(m_SpawnDelay);

        m_Lock = false;
        m_IsSpawning = false;
        m_Wave.AttemptZombieSpawn();
        m_Lock = false;
    }

    public bool IsSpawning
    {
        get { return m_IsSpawning; }

    }

    public bool IsLocked
    {
        get { return m_Lock; }
    }

  
}
