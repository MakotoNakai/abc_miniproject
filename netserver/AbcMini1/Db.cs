using Google.Cloud.Firestore;

namespace AbcMini1; 

public static class Db {
    public static FirestoreDb Instance { get; private set; }
    
    public static void Initialize() {
        Instance = FirestoreDb.Create("abc-minipro-18-stg");
    }
}