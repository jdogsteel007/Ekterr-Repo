using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class Timeline : MonoBehaviour
{
    public bool ReopenWindow = false; //If this is true, it means the window was closed automatically and should re-open. 
    [HideInInspector]
    public bool IsPaused = false;
    public bool PlayOnStart = false;
    public bool IsPlaying { get; private set; }
    public float PlayTime { get; private set; }

    [HideInInspector]
    public List<TimelineEvent> Events = new List<TimelineEvent>();
    public TimelineEvent LastClickedEvent;

    private void Start()
    {
        if (PlayOnStart)
            Play();
    }
    public void Play() { StartCoroutine(PlayTimeline()); }
    public void Stop() { StopCoroutine(PlayTimeline()); }

    public IEnumerator PlayTimeline()
    {
        IsPlaying = true;
        if (Events != null && Events.Count > 0)
        {
            List<TimelineEvent> activeEvents = new List<TimelineEvent>();
            int lastIndex = 0;

            while (lastIndex < Events.Count || activeEvents.Count > 0)
            {
                while (IsPaused)
                    yield return null;

                //Step further through the timeline every frame:
                PlayTime += Time.deltaTime;
                if (lastIndex < Events.Count && Events[lastIndex].Position < PlayTime)
                {
                    Events[lastIndex].OnEnter.Invoke();
                    activeEvents.Add(Events[lastIndex]);
                    lastIndex++;
                }
                if (activeEvents.Count > 0)
                {
                    for (int i = 0; i < activeEvents.Count; i++)
                    {
                        if (PlayTime > activeEvents[i].Position + activeEvents[i].Length)
                        {
                            activeEvents[i].OnExit.Invoke();
                            activeEvents.Remove(activeEvents[i]);
                        }
                    }
                }
                yield return null;
            }
        }
        IsPlaying = false;
    }
}

[CustomEditor(typeof(Timeline))]
public class TimelineInspector : Editor
{
    TimelineEditorWindow win;
    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("Open editor window") || (win == null && (target as Timeline).ReopenWindow))
        {
            win = TimelineEditorWindow.GetEditorWindow((target as Timeline));
            (target as Timeline).ReopenWindow = true;
        }
        //(target as Timeline).PlayOnStart = GUILayout.Toggle((target as Timeline).PlayOnStart, "Play On Start");
        DrawDefaultInspector();
    }
}

public class TimelineEditorWindow : EditorWindow
{
    public static TimelineEditorWindow GetEditorWindow(Timeline target)
    {
        TimelineEditorWindow win = GetWindow<TimelineEditorWindow>();
        win.Target = target;
        return win;
    }

    public Timeline Target;
    private TimelineEvent _movingEvent;

    private void OnDestroy()
    {
        Target.ReopenWindow = false;
    }

    private void OnGUI()
    {
        CheckInput();
        DrawTimelineArea();
        DrawPropertiesArea();
    }

    private void CheckInput()
    {
        Vector2 mPos = Event.current.mousePosition;
        if (_resizingPropertiesArea)
        {
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)   //mouse button was released
                _resizingPropertiesArea = false;
            else if (Event.current.type == EventType.MouseDrag)
            {
                {
                    EditorPropertiesSize -= Event.current.delta.x;
                    Repaint();
                }
            }
        }
        else if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && _propertiesResizeHandleRect.Contains(Event.current.mousePosition)) //we clicked on the timeline resize rect
            _resizingPropertiesArea = true;

        if (TimelineArea.Contains(mPos))
        {
            if(_movingEvent != null)
            {
                if (Event.current.type == EventType.MouseUp && Event.current.button == 0)   //mouse button was released
                    _movingEvent = null;
                else if (Event.current.type == EventType.MouseDrag)
                {
                    _movingEvent.HandleDrag(this);
                }
            }
            else if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0)  //Means we clicked somewhere
                    foreach (TimelineEvent tl in Target.Events)
                    {
                        bool click = tl.CheckClick(this);
                        if (click)
                        {
                            Target.LastClickedEvent = tl;
                            _movingEvent = tl;
                            Repaint();
                            break;
                        }
                    }
                else if (Event.current.button == 1)  //Context menu
                {
                    GenericMenu menu = new GenericMenu();
                    bool clickedOnEvent = false;
                    foreach (TimelineEvent tl in Target.Events)
                    {
                        if (tl.CheckClick(this))
                        {
                            menu.AddItem(new GUIContent("Remove event"), false, () =>
                            {
                                Target.Events.Remove(tl);
                                if (Target.LastClickedEvent == tl)
                                    Target.LastClickedEvent = null;
                                Target.LastClickedEvent = null;
                            });
                            menu.ShowAsContext();
                            clickedOnEvent = true;
                            break;
                        }
                    }
                    if (!clickedOnEvent)
                    {
                        menu.AddItem(new GUIContent("Add new event"), false, () =>
                        {
                            TimelineEvent tle = new TimelineEvent();
                            tle.Position = (mPos.x / SecondSize) + ScrollOffset;
                            tle.Track = ((int)mPos.y - EventRectHeight) / EventRectHeight;
                            Target.Events.Add(tle);
                            Target.LastClickedEvent = tle;
                            Repaint();
                        });
                        menu.ShowAsContext();
                    }
                }
            }
            else if (Event.current.type == EventType.MouseDrag && (Event.current.control && Event.current.button == 0) || Event.current.button == 2) //pan the view
            {
                ScrollOffset -= Event.current.delta.x;
                if (ScrollOffset < -100) ScrollOffset = -100;
                Repaint();
            }
            else if (Event.current.type == EventType.ScrollWheel)   //zoom
            {
                if (SecondSize + Event.current.delta.y < 5)
                    SecondSize = 5;
                else
                {
                    SecondSize += Event.current.delta.y;
                    ScrollOffset += Event.current.delta.y;
                }
                Repaint();
            }
        }
        else if(_movingEvent != null) { _movingEvent = null; }
    }

    private void DrawTimelineArea()
    {
        GUILayout.BeginArea(TimelineArea);
        try
        {
            EditorGUI.DrawRect(TimelineArea, new Color(0.2f, 0.2f, 0.2f));  //Background
            Handles.color = Color.grey;
            Handles.DrawLines(TimelineGridLines);                           //BackgroundLines

            EditorGUI.LabelField(new Rect(0, 5, 100, 8), Target.gameObject.name + "    Right click to add new events.", new GUIStyle() { fontStyle = FontStyle.Bold, normal = new GUIStyleState() { textColor = (Target.IsPlaying ? Color.green : Color.white) } });
            foreach (TimelineEvent tl in Target.Events) //Draw events
            {
                tl.Draw(this);
            }

            if (Target.IsPlaying) //Draw the cursor
            {
                Handles.color = Color.green;
                float xpos = Target.PlayTime * SecondSize - ScrollOffset;
                Handles.DrawLine(new Vector3(xpos, 0), new Vector3(xpos, position.height));
                Repaint();
            }
        }
        catch (System.Exception e)   //Exeption here means gameobject needs to be refreshed, same with other exception below
        {
            if (e is System.NullReferenceException) //We can ignore exceptions thrown by color picker and object widgets, not sure what's up with those anyway
            {
                Debug.LogError(e.Message);
            }
        }
        finally
        {
            GUILayout.EndArea();
        }
    }

    private void DrawPropertiesArea()
    {
        if (Target.LastClickedEvent !=  null)
        {
            if (_propertiesResizeHandleRect.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(_propertiesResizeHandleRect, new Color(0.0f, 0.0f, 0.8f));
                Repaint();
            }
            else EditorGUI.DrawRect(_propertiesResizeHandleRect, new Color(0.8f, 1f, 0.8f));

            Rect pArea = PropertiesArea;
            pArea.x += 10;
            pArea.width -= 10;

            GUILayout.BeginArea(pArea);
            try
            {
                Target.LastClickedEvent.Name = EditorGUILayout.TextField("Name", Target.LastClickedEvent.Name);
                Target.LastClickedEvent.BackgroundColor = EditorGUILayout.ColorField("Color", Target.LastClickedEvent.BackgroundColor);
                Target.LastClickedEvent.Position = EditorGUILayout.FloatField("Position", Target.LastClickedEvent.Position);
                Target.LastClickedEvent.Length = EditorGUILayout.FloatField("Length", Target.LastClickedEvent.Length);
                Target.LastClickedEvent.Track = EditorGUILayout.IntField("Track", Target.LastClickedEvent.Track);

                EditorGUILayout.LabelField("See inspector for event values.");

                /*  //Can't draw unityevents in here, I tried everything and nothing seems to work properly
                SerializedObject LCEvent = new SerializedObject(Target);

                EditorGUILayout.PropertyField(LCEvent.FindProperty("LastClickedEvent.OnEnter"), new GUIContent("On Enter"));
                EditorGUILayout.PropertyField(LCEvent.FindProperty("LastClickedEvent.OnExit"), new GUIContent("On Exit"));

                LCEvent.ApplyModifiedProperties();
                LCEvent.Update();
                */

                Repaint();
            }
            catch (System.Exception e)
            {
                if (e is System.NullReferenceException) //We can ignore exceptions thrown by color picker and object widgets, not sure what's up with those anyway
                {
                    Debug.LogError(e.Message);
                }
            }
            finally
            {
                GUILayout.EndArea();
            }
        }
    }

    //BELOW: Drawing variables

    public Vector3[] TimelineGridLines
    {
        get
        {
            List<Vector3> vecs = new List<Vector3>();
            for (float i = 0; i < position.width - PropertiesArea.width + ScrollOffset; i += SecondSize)
            {
                vecs.Add(new Vector3(i - ScrollOffset, 0));
                vecs.Add(new Vector3(i - ScrollOffset, position.height));
            }
            for (float i = 0; i < position.height; i += EventRectHeight)
            {
                vecs.Add(new Vector3(0, i));
                vecs.Add(new Vector3(position.width - PropertiesArea.width, i));
            }
            return vecs.ToArray();
        }
    }

    public float EditorPropertiesSize
    { //Size of property area on the right hand side of the window
        get { return _editorPropertiesSize; }
        set
        {
            if (value < 50)
                _editorPropertiesSize = 50;
            else if (value > position.width - 50)
                _editorPropertiesSize = position.width - 50;
            else
                _editorPropertiesSize = value;
        }
    }

    public Rect TimelineArea { get { return new Rect(0, 0, position.width - EditorPropertiesSize, position.height); } } //Area where timeline controls are contained
    public Rect PropertiesArea { get { return new Rect(position.width - EditorPropertiesSize, 0, EditorPropertiesSize, position.height); } } //Area where timeline properties are contained

    private Rect _propertiesResizeHandleRect { get { return new Rect(position.width - EditorPropertiesSize - 5, 0, 10, position.height); } }
    private bool _resizingPropertiesArea = false;
    private float _editorPropertiesSize = 300;

    //Static below

    public static float ScrollOffset
    {
        get { return _scrollOffset; }
        set
        {
            if (value != ScrollOffset)
            {
                _scrollOffset = value;
            }
        }
    }

    public static float SecondSize = 50; // 1 Second is 50 pixels wide by default.
    public static int EventRectHeight = 30;

    private static float _scrollOffset = 0;

}

[System.Serializable]
public class TimelineEvent// : ScriptableObject
{
    [SerializeField]
    private float _position = 0, _length = 1;
    [SerializeField]
    private int _track = 0;

    public string Name = "New Event";
    public Color BackgroundColor = Color.blue;
    public UnityEvent OnEnter = new UnityEvent(), OnExit = new UnityEvent();
    public float Position
    {
        get { return _position; }
        set
        {
            if (value < 0)
                _position = 0;
            else
                _position = value;
        }
    }
    public float Length
    {
        get { return _length; }
        set
        {
            if (value < 0.5)
                _length = 0.5f; //Min length is half a second
            else
                _length = value;
        }
    }
    public int Track
    {
        get { return _track; }
        set
        {
            if (value < 0)
                _track = 0;
            else
                _track = value;
        }
    }
    public Rect ToolRect { get { return new Rect(
        Position * TimelineEditorWindow.SecondSize - TimelineEditorWindow.ScrollOffset, 
        (Track + 1) * TimelineEditorWindow.EventRectHeight, 
        Length * TimelineEditorWindow.SecondSize, 
        TimelineEditorWindow.EventRectHeight);
        } }

    public void Draw(TimelineEditorWindow editor)
    {
        EditorGUI.DrawRect(ToolRect, BackgroundColor);  //Draw the background first
        GUILayout.BeginArea(ToolRect);
        GUILayout.Label(Name, new GUIStyle() { fontStyle = FontStyle.Bold, normal = new GUIStyleState() { textColor = Color.white } });
        GUILayout.Label((editor.Target as Timeline).Events.IndexOf(this).ToString(), new GUIStyle() { fontStyle = FontStyle.Bold, normal = new GUIStyleState() { textColor = Color.white } });
        GUILayout.EndArea();
    }
    public bool CheckClick(TimelineEditorWindow editor)
    {
        Vector2 mousePos = Event.current.mousePosition;
        if (ToolRect.Contains(mousePos))
        {
            editor.Repaint();
            return true;
        }
        return false;
    }
    public void HandleDrag(TimelineEditorWindow editor)
    {
        Position = Position + Event.current.delta.x / TimelineEditorWindow.SecondSize;

        //Rearrage the event in the list so they stay chronologically ordered
        int index = editor.Target.Events.IndexOf(this);
        if (editor.Target.Events.Count - 1 > index && Position > editor.Target.Events[index + 1].Position)
        {
            editor.Target.Events.RemoveAt(index);
            editor.Target.Events.Insert(index + 1, this);
        }
        else if (index > 0 && Position < editor.Target.Events[index - 1].Position)
        {
            editor.Target.Events.RemoveAt(index);
            editor.Target.Events.Insert(index - 1, this);
        }

        Vector2 mPos = Event.current.mousePosition;
        if (mPos.y > ((Track + 2) * TimelineEditorWindow.EventRectHeight))
            Track++;
        else if (mPos.y < (Track + 1) * TimelineEditorWindow.EventRectHeight)
            Track--;

        editor.Repaint();
    }
}