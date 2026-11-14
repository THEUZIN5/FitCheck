using UnityEngine;
namespace FitCheck
{
    // One transient mesh/material; no lights or permanent city actors.
    public sealed class WeekFireworks : MonoBehaviour
    {
        const int Bursts=5,Sparks=64,Trails=4,Quads=Bursts*(Sparks*Trails+1);
        readonly Vector3[] vertices=new Vector3[Quads*4],radii=new Vector3[Quads*4];
        readonly Color[] colors=new Color[Quads*4];
        readonly float[] starts={.65f,1.55f,2.55f,3.55f,4.6f};
        readonly Vector3[] centers={new Vector3(-3.8f,8,5),new Vector3(3.6f,9,6),new Vector3(0,10.2f,7),new Vector3(-3.3f,9.3f,6),new Vector3(3.5f,8.4f,5)};
        readonly Color[] palette={new Color(1,.04f,.09f),new Color(1,.71f,.16f),new Color(.13f,.87f,.9f),new Color(.78f,.34f,1),new Color(1,.83f,.35f)};
        Mesh mesh;Material material;
        public int VisibleSparks {get;private set;}
        public void Build(){
            mesh=new Mesh{name="Fogos da semana — malha compartilhada"};mesh.MarkDynamic();
            material=new Material(Resources.Load<Shader>("CityLightHalo"));
            var uv=new Vector2[vertices.Length];var indices=new int[Quads*6];
            for(int q=0;q<Quads;q++){
                int v=q*4,t=q*6;uv[v]=Vector2.zero;uv[v+1]=Vector2.right;uv[v+2]=Vector2.one;uv[v+3]=Vector2.up;
                indices[t]=v;indices[t+1]=v+1;indices[t+2]=v+2;indices[t+3]=v;indices[t+4]=v+2;indices[t+5]=v+3;
            }
            mesh.vertices=vertices;mesh.normals=radii;mesh.colors=colors;mesh.uv=uv;mesh.triangles=indices;
            mesh.bounds=new Bounds(new Vector3(0,6,5),Vector3.one*30);
            gameObject.AddComponent<MeshFilter>().sharedMesh=mesh;
            var renderer=gameObject.AddComponent<MeshRenderer>();renderer.sharedMaterial=material;
            renderer.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;renderer.receiveShadows=false;
        }
        void Quad(int q,Vector3 center,float radius,Color color){
            for(int i=0;i<4;i++){int v=q*4+i;vertices[v]=center;radii[v]=new Vector3(radius,0,0);colors[v]=color;}
        }
        public void RenderAt(float clock,bool reduced){
            VisibleSparks=0;System.Array.Clear(colors,0,colors.Length);
            for(int b=0;b<Bursts;b++){
                float age=clock-starts[b];int offset=b*(Sparks*Trails+1);
                if(reduced){
                    float alpha=Mathf.SmoothStep(0,1,clock/1.5f)*(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(5.5f,7.4f,clock)));
                    if(b>2)continue;
                    for(int s=0;s<20;s++){float a=s*Mathf.PI*.1f;Quad(offset+s,new Vector3(centers[b].x+Mathf.Cos(a)*1.8f,centers[b].y+Mathf.Sin(a)*1.8f,centers[b].z),.12f,new Color(palette[b].r,palette[b].g,palette[b].b,alpha*.7f));VisibleSparks++;}
                    continue;
                }
                if(age>-.5f&&age<0){var color=palette[b];color.a=Mathf.Clamp01((age+.5f)*4);Quad(offset+Sparks*Trails,centers[b]+Vector3.down*(-age*8),.13f,color);}
                if(age<0||age>2.1f)continue;
                for(int s=0;s<Sparks;s++){
                    float y=1-2*(s+.5f)/Sparks,a=s*2.399963f+b*.7f,r=Mathf.Sqrt(1-y*y);
                    var direction=new Vector3(Mathf.Cos(a)*r,y,Mathf.Sin(a)*r);
                    float speed=3.5f+.8f*Mathf.Sin(s*17.1f);
                    for(int trail=0;trail<Trails;trail++){
                        float t=Mathf.Max(0,age-trail*.085f),spread=speed*(1-Mathf.Exp(-t*1.7f));
                        var position=centers[b]+direction*spread+Vector3.down*(t*t*.65f);
                        var color=Color.Lerp(palette[b],Color.white,Mathf.Clamp01(1-age*4)*.65f);
                        color.a=Mathf.Clamp01((2.1f-age)/1.4f)*(1-trail*.18f);
                        Quad(offset+s*Trails+trail,position,.22f-trail*.035f,color);VisibleSparks++;
                    }
                }
            }
            mesh.vertices=vertices;mesh.normals=radii;mesh.colors=colors;
        }
        void OnDestroy(){if(mesh!=null)Destroy(mesh);if(material!=null)Destroy(material);}
    }
}
