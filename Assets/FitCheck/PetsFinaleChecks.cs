using System;
using System.IO;
using System.Linq;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckPetsFinale(string folder){
            storageOverride=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));Directory.CreateDirectory(storageOverride);
            Home();
            string[] legacy={"Vira-lata","Shiba","Dachshund"};
            for(int i=0;i<legacy.Length;i++)if(PetAppearance.DogBreeds[i]!=legacy[i])throw new Exception("IDs antigos das raças mudaram");
            int combinations=0,maxVertices=0;
            for(int breed=0;breed<PetAppearance.DogBreeds.Length;breed++)for(int coat=0;coat<6;coat++){
                var choice=new PetAppearance{species=1,breed=breed,coat=coat,name="Amigo"};choice.Validate();
                var appearance=playerAppearance.Copy();appearance.pet=choice;
                string path=Path.Combine(folder,"isolated-pet-"+breed+".json");AppearanceStore.Save(path,appearance);
                var loaded=AppearanceStore.Load(path);if(loaded.pet.breed!=breed||loaded.pet.coat!=coat||loaded.pet.species!=1)throw new Exception("Raça perdida no save");
                var root=new GameObject("Verificação de raça");root.transform.position=Vector3.right*1800;
                var pet=root.AddComponent<ShopCat>();pet.Build(loaded.pet);if(!pet.Ready||pet.IsFuba)throw new Exception("Companheiro confundido com Fubá");
                float age=pet.Age;pet.Tick(.2f,true,false,true);if(pet.Age!=age)throw new Exception("Pausa move o cachorro");
                pet.Tick(.2f,false,false,true);pet.Tick(.2f,false,true,true);
                int vertices=pet.GetComponentsInChildren<MeshFilter>().Sum(f=>f.sharedMesh.vertexCount);maxVertices=Math.Max(maxVertices,vertices);
                if(vertices>18000)throw new Exception("Raça excede o orçamento de geometria");
                root.SetActive(false);Destroy(root);combinations++;
            }
            OpenCreator(Stage.Home);creatorPetTab=true;
            for(int breed=3;breed<=5;breed++){
                creatorDraft.pet=new PetAppearance{species=1,breed=breed,coat=breed==3?1:breed==4?4:0,name=PetAppearance.DogBreeds[breed]};RefreshCreatorPet();
                creatorPetPreview.Tick(.3f,false,false,true);creatorPetPreview.transform.localRotation=Quaternion.identity;
                cam.transform.position=studio.transform.position+new Vector3(-1.4f,1.05f,-2.7f);cam.transform.LookAt(studio.transform.position+new Vector3(-.05f,.40f,0));cam.fieldOfView=41;
                SaveFirstPersonFrame(Path.Combine(folder,"dog-"+breed+".png"),1440,900);
            }
            CancelCreator();Home();
            progress=new DayProgress();var solutions=File.ReadAllLines(Path.Combine(folder,"solutions.txt")).Select(line=>line.Split('|')).ToArray();
            for(int day=1;day<=7;day++){for(int j=0;j<3;j++)progress.Add(new Attempt{outfit=solutions[progress.posts.Count]});progress.Close();if(day<7)progress.Advance();}
            stage=Stage.Summary;string before=JsonUtility.ToJson(progress);var cameraBefore=cam.transform.position;float fovBefore=cam.fieldOfView;
            BeginWeekCelebration();if(!weekCelebrating||weekFireworks==null)throw new Exception("Sem celebração após 21 encontros");
            for(int i=0;i<32;i++)TickWeekCelebration(.1f);
            if(weekFireworks.VisibleSparks<100)throw new Exception("Fogos invisíveis");
            SaveFirstPersonFrame(Path.Combine(folder,"finale-fireworks.png"),1440,900);
            paused=true;float clock=weekCelebrationClock;TickWeekCelebration(.5f);if(clock!=weekCelebrationClock)throw new Exception("Pausa avançou os fogos");paused=false;
            reducedMotion=true;FrameWeekCelebration();if(weekFireworks.VisibleSparks!=60)throw new Exception("Movimento reduzido não usa constelações estáticas");
            SaveFirstPersonFrame(Path.Combine(folder,"finale-reduced.png"),1440,900);reducedMotion=false;
            FinishWeekCelebration();if(weekCelebrating||weekCelebrationScene!=null||stage!=Stage.WeekMemory||!File.Exists(WeekPhotoPath))throw new Exception("Final não guardou a foto");
            if(Vector3.Distance(cam.transform.position,cameraBefore)>.0001f||!Mathf.Approximately(cam.fieldOfView,fovBefore))throw new Exception("Câmera não foi restaurada");
            if(JsonUtility.ToJson(progress)!=before)throw new Exception("Celebração alterou a campanha");
            byte[] first=File.ReadAllBytes(WeekPhotoPath);CloseWeekMemory();
            BeginWeekCelebration();if(weekCelebrating||stage!=Stage.WeekMemory||!first.SequenceEqual(File.ReadAllBytes(WeekPhotoPath)))throw new Exception("Reabertura repete fogos ou sobrescreve a foto");
            CloseWeekMemory();storageOverride=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));Directory.CreateDirectory(storageOverride);
            BeginWeekCelebration();for(int tick=0;tick<80&&weekCelebrating;tick++)TickWeekCelebration(.1f);
            if(weekCelebrating||stage!=Stage.WeekMemory||!File.Exists(WeekPhotoPath))throw new Exception("Fim automático da celebração falhou");
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"PETS_FINALE_PASSED: "+combinations+" breed/coat/save combinations, legacy IDs, pause/reduced motion, max "+maxVertices+" vertices per dog, complete 21-encounter celebration, skip, camera restore, photo preservation and unchanged campaign.");
            Debug.Log(File.ReadAllText(Path.Combine(folder,"checks.txt")));
        }
    }
}
