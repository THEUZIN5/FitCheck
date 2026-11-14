using System;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void AnimateWeekPortrait(float delta){
            for(int i=0;i<residents.Count;i++){var actor=residents[i];actor.cycle+=delta;actor.blinkClock+=delta;PoseResident(actor,i,0);}
        }
    }
    public partial class FirstCustomer
    {
        bool weekCelebrating;float weekCelebrationClock;
        CityView weekCelebrationScene;WeekFireworks weekFireworks;
        Vector3 weekCameraPosition;Quaternion weekCameraRotation;
        float weekCameraFov,weekCameraFar,weekCameraNear;CameraClearFlags weekCameraFlags;
        Color weekCameraColor;bool weekSkyEnabled;
        const float WeekCelebrationSeconds=7.4f;
        void BeginWeekCelebration(){
            if(!progress.Complete||LoadWeekMemory()!=null){OpenWeekMemory();return;}
            if(weekCelebrating)return;
            weekCameraPosition=cam.transform.position;weekCameraRotation=cam.transform.rotation;weekCameraFov=cam.fieldOfView;weekCameraFar=cam.farClipPlane;weekCameraNear=cam.nearClipPlane;weekCameraFlags=cam.clearFlags;weekCameraColor=cam.backgroundColor;
            var sky=cam.GetComponent<Skybox>();weekSkyEnabled=sky!=null&&sky.enabled;
            try{
                weekCelebrationScene=new GameObject("Celebração dos 21 encontros — temporária").AddComponent<CityView>();
                weekCelebrationScene.ComposeWeekPortrait(studio,progress,playerAppearance);
                var root=new GameObject("Fogos do bairro");root.transform.SetParent(weekCelebrationScene.transform,false);
                weekFireworks=root.AddComponent<WeekFireworks>();weekFireworks.Build();
                weekCelebrationClock=0;weekCelebrating=true;paused=false;dragging=false;cityFreeCursor=true;arrivalFade=photoFlash=0;studio.Talking=false;
                if(city!=null&&city.Cat!=null)city.TickCat(0,true,reducedMotion,mutedAudio);
                SyncCityCursor(true);FrameWeekCelebration();
            }catch(Exception error){DisposeWeekCelebration();Debug.LogWarning("Celebração indisponível; abrindo a lembrança. "+error.Message);OpenWeekMemory();}
        }
        void FrameWeekCelebration(){
            float t=reducedMotion?1:Mathf.SmoothStep(0,1,weekCelebrationClock/WeekCelebrationSeconds);
            cam.clearFlags=CameraClearFlags.SolidColor;cam.backgroundColor=new Color(.009f,.013f,.028f);cam.nearClipPlane=.1f;cam.farClipPlane=45;cam.fieldOfView=46;
            var sky=cam.GetComponent<Skybox>();if(sky!=null)sky.enabled=false;
            var origin=weekCelebrationScene.transform.position;
            cam.transform.position=origin+new Vector3(0,4.4f,Mathf.Lerp(-20,-18,t));cam.transform.LookAt(origin+new Vector3(0,4.5f,1.5f));
            weekFireworks.RenderAt(weekCelebrationClock,reducedMotion);
        }
        void TickWeekCelebration(float delta){
            var retro=cam.GetComponent<RetroCamera>();retro.Paused=paused;retro.ReducedMotion=reducedMotion;
            TickPresentation(delta);
            if(paused)return;
            weekCelebrationClock+=Mathf.Min(delta,.1f);weekCelebrationScene.AnimateWeekPortrait(Mathf.Min(delta,.05f));FrameWeekCelebration();
            if(weekCelebrationClock>=WeekCelebrationSeconds)FinishWeekCelebration();
        }
        void FinishWeekCelebration(){DisposeWeekCelebration();paused=false;OpenWeekMemory();PlayClip(Clip("photo_capture")??photoSound,.65f);}
        void DisposeWeekCelebration(){
            bool restore=weekCelebrating;weekCelebrating=false;
            if(weekCelebrationScene!=null){weekCelebrationScene.gameObject.SetActive(false);Destroy(weekCelebrationScene.gameObject);}weekCelebrationScene=null;weekFireworks=null;
            if(restore&&cam!=null){cam.transform.SetPositionAndRotation(weekCameraPosition,weekCameraRotation);cam.fieldOfView=weekCameraFov;cam.farClipPlane=weekCameraFar;cam.nearClipPlane=weekCameraNear;cam.clearFlags=weekCameraFlags;cam.backgroundColor=weekCameraColor;var sky=cam.GetComponent<Skybox>();if(sky!=null)sky.enabled=weekSkyEnabled;}
        }
        void DrawWeekCelebration(){
            var matrix=GUI.matrix;var color=GUI.color;var content=GUI.contentColor;var background=GUI.backgroundColor;bool enabled=GUI.enabled;
            try{
                GUI.enabled=true;GUI.color=GUI.contentColor=GUI.backgroundColor=Color.white;
                GUI.matrix=Matrix4x4.identity;cam.GetComponent<RetroCamera>().DrawWorld();InterfaceStyles();
                float scale=Mathf.Min(Screen.width/1440f,Screen.height/900f);
                Fill(new Rect(0,0,Screen.width,65*scale),Color.black);Fill(new Rect(0,Screen.height-98*scale,Screen.width,98*scale),Color.black);
                GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1440*scale)/2,(Screen.height-900*scale)/2,0),Quaternion.identity,Vector3.one*scale);
                Txt(new Rect(48,10,1344,48),paused?"Em pausa":"21 encontros. O bairro é parte dessa história.",action,paper);
                Txt(new Rect(48,821,800,40),"Uma semana de estilo merece uma lembrança.",copy,paper);
                if(paused&&Tap(new Rect(880,822,220,45),"Continuar",true))paused=false;
                if(Tap(new Rect(1120,822,272,45),"Ir para a foto →",false))FinishWeekCelebration();
                var e=Event.current;
#if !UNITY_WEBGL || UNITY_EDITOR
                if(e.type==EventType.KeyDown&&(e.keyCode==KeyCode.P||e.keyCode==KeyCode.Escape)){paused=!paused;e.Use();}
#endif
            }finally{GUI.matrix=matrix;GUI.color=color;GUI.contentColor=content;GUI.backgroundColor=background;GUI.enabled=enabled;}
        }
    }
}
