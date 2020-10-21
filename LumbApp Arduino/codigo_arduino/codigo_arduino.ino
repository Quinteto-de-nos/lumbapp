
#define L2 2
#define L3Arriba 3
#define L3Abajo 4
#define L4ArribaIzquierda 5
#define L4ArribaCentro 6
#define L4ArribaDerecha 7
#define L4Abajo 8
#define Duramadre 9
#define L5 10
#define TejidoAdiposo 11

#define echoPin 13
char estado_puertos_old [11] = {"2222222222$"};
char estado_puertos [11] = {"0000000000$"}; //inicializo el vector de estados
char recibido = 0;
bool iniciamos = false;

void setup() {

pinMode(2, INPUT_PULLUP);    // L2
pinMode(3, INPUT_PULLUP);    // L3 arriba
pinMode(4, INPUT_PULLUP);    // L3 abajo
pinMode(5, INPUT_PULLUP);    // L4 arriba izq
pinMode(6, INPUT_PULLUP);    // L4 arriba arriba centro
pinMode(7, INPUT_PULLUP);    // L4 arriba derecha
pinMode(8, INPUT_PULLUP);    // L4 abajo
pinMode(9, INPUT_PULLUP);    // duramadre
pinMode(10, INPUT_PULLUP);    // L5
pinMode(11, INPUT_PULLUP);    // Tejido Adipooso


  Serial.begin(9600);


}

void loop() {
 /* if(iniciamos== false){
  Serial.println("soy false");
  }
  else{
  Serial.println("soy true");
  }*/
  if (iniciamos == true){
    
        
        estado_puertos[0] = (char) (digitalRead(TejidoAdiposo) + 48);
        estado_puertos[1] = (char) (digitalRead(L2) + 48);
        estado_puertos[2] = (char) (digitalRead(L3Arriba) + 48);
        estado_puertos[3] = (char) (digitalRead(L3Abajo) + 48);
        estado_puertos[4] = (char) (digitalRead(L4ArribaIzquierda) + 48);
        estado_puertos[5] = (char) (digitalRead(L4ArribaDerecha) + 48);
        estado_puertos[6] = (char) (digitalRead(L4ArribaCentro) + 48);
        estado_puertos[7] = (char) (digitalRead(L4Abajo) + 48);
        estado_puertos[8] = (char) (digitalRead(L5) + 48);
        estado_puertos[9] = (char) (digitalRead(Duramadre) + 48);

       // if (strcmp (estado_puertos_old, estado_puertos) != 0 ){
        if (estado_puertos_old[0] != estado_puertos[0] || estado_puertos_old[1] != estado_puertos[1] || estado_puertos_old[2] != estado_puertos[2] || estado_puertos_old[3] != estado_puertos[3] || estado_puertos_old[4] != estado_puertos[4] || estado_puertos_old[5] != estado_puertos[5] || estado_puertos_old[6] != estado_puertos[6] || estado_puertos_old[7] != estado_puertos[7] || estado_puertos_old[8] != estado_puertos[8] || estado_puertos_old[9] != estado_puertos[9]){          
            Serial.println(estado_puertos);
            strcpy(estado_puertos_old, estado_puertos);
        }
 
        delay(250);
  }
  
                
    recibido = (char)Serial.read();

    switch(recibido){
    case 'p':                               //señal para Prender Arduino
              Serial.println("0000$");      //ack
              iniciamos = true;
              break;
              
    case 'a':                             //señal de Apagar Arduino
              Serial.println("8888$");
              iniciamos = false;          //ack
              break;
              
    default: 
             
              
              break;
    }  

}    










/*
    

    if(recibido  == 'p'){       //Si recibo 'p', envío '0000' como respuesta e iniciamos
      
              Serial.println("0000$");
              iniciamos = true;
      
    }
    else
    {
    
              if(recibido  == 'a'){       //Si recibo 'a', envío '8888' como respuesta y apagamos Arduino
                
                Serial.println("8888$");
                iniciamos = false;
                
              }

    
    
              else{     // Si no recibo 'a', envío '9999' (un string que no contenga 0s ó 1s)
               // Serial.println("waiting$");
                Serial.println ("9999$");
                }
    }


 
}    */
