using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;

public class RadialMultiValueSlider : UIBehaviour,  IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [System.Serializable]
    public class Value
    {
        public string name;
        public Color color;
        public float val;
        [System.NonSerialized]
        public Image image;
        [System.NonSerialized]
        public RectTransform knob;
        [System.NonSerialized]
        public Value next;
        public float actualValue
        {
            get
            {
                if (next == null || next == this)
                    return val;
                float v = next.val - val;
                if (v < 0)
                    v += 1;
                return v;
            }
        }
    }
    public Sprite circleSprite;
    public Sprite knob;
    public bool colorKnob = true;
    public float radius = 50;
    public List<Value> values = new List<Value> {
        new Value { name="red", color = Color.red, val = 0},
        new Value { name="green", color = Color.green, val = 0.2f},
        new Value { name="blue", color = Color.blue, val = 0.4f},
        new Value { name="magenta", color = Color.magenta, val = 0.7f},
    };

    
    private Value m_dragObj = null;
    private List<Value> m_Values = new List<Value>();


	protected override void Start ()
    {
        base.Start();
        UpdateChilds();
    }

    [ContextMenu("PrintValues")]
    void PrintValues()
    {
        foreach(var v in values)
        {
            Debug.Log(""+v.name + " = " + v.actualValue);
        }
    }
    [ContextMenu("UpdateChilds")]
    public void UpdateChilds()
    {
        foreach(var t in transform.Cast<Transform>().ToList())
        {
            Destroy(t.gameObject);
        }
        m_Values.Clear();
        for (int i = 0; i < values.Count; i++)
        {
            var v = values[i];
            v.image = new GameObject("arc").AddComponent<Image>();
            v.image.transform.SetParent(transform, false);
            v.image.sprite = circleSprite;
            v.image.type = Image.Type.Filled;
            v.image.fillMethod = Image.FillMethod.Radial360;
            v.image.fillOrigin = 1;
            v.image.fillClockwise = false;
            v.image.color = v.color;
            m_Values.Add(v);
        }
        for (int i = 0; i < values.Count; i++)
        {
            var v = values[i];
            var tmp = new GameObject("knob").AddComponent<Image>();
            tmp.sprite = knob;
            if (colorKnob)
                tmp.color = v.color;
            v.knob = tmp.transform as RectTransform;
            v.knob.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);
            v.knob.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
            v.knob.SetParent(transform, false);
            v.knob.localPosition += Vector3.right * radius - Vector3.forward;

        }
    }

    void Update()
    {
        m_Values.Sort((a,b)=>a.val.CompareTo(b.val));
        Value last = m_Values[m_Values.Count - 1];
        for(int i = 0; i < m_Values.Count; i++)
        {
            var v = m_Values[i];
            var vn = m_Values[(i+1) % values.Count];
            v.image.transform.localRotation = Quaternion.Euler(0,0,v.val*360);
            v.knob.localPosition = v.image.transform.localRotation * (Vector3.right * radius);
            float a = vn.val - v.val;
            if (a < 0)
                a += 1f;
            v.image.fillAmount = a;
            last.next = v;
            last = v;
        }
    }

    float GetAngle(PointerEventData eventData)
    {
        var v = eventData.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(v.y, v.x) / (Mathf.PI * 2);
        if (angle < 0)
            angle += 1f;
        return angle;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var dir = eventData.position - (Vector2)transform.position;
        m_dragObj = null;
        if (Mathf.Abs(dir.magnitude - radius) > 7)
            return;
        float angle = GetAngle(eventData);
        float dif = 100;
        for (int i = 0; i < values.Count; i++)
        {
            var v = values[i];
            var d = Mathf.Abs(v.val - angle);
            if (d > 0.5f)
                d = 1f - d;
            if (d < dif)
            {
                dif = d;
                m_dragObj = v;
            }
        }
        OnDrag(eventData);
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (m_dragObj != null)
        {
            float angle = GetAngle(eventData);
            m_dragObj.val = angle;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_dragObj = null;
    }

}
