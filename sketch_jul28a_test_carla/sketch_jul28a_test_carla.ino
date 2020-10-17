
//char estado_puertos [11] = {"1111111111$"};
char prueba;
char recibido = 0;

bool iniciamos = false;
void setup() {


Serial.begin(9600);

}

void loop() {
  if (iniciamos == true){
            //Serial.println("arrancamo$");
             
              }

    else{
                
                recibido = (char)Serial.read();
                
                if(recibido  == 'p'){
                  
                  Serial.println("2222$");
                  iniciamos = true;
                  
                }
            
              else{
               // Serial.println("waiting$");
                Serial.println ("0000$");
              }
            
            }

}
