namespace SistemaGeneral.Utility {
    public class ExceptionsHandler {

        public static void HandleException() { 
            
        }

        public void Ejecutar<T>(T metodo) where T : Delegate {
            try {
                metodo.DynamicInvoke();
            }
            catch (Exception ex) { 
                
            }
        }
    }
}
