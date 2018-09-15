#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>

int upPin = 2; //d4
int leftPin = 4; //d2
int centerPin = 5; //d1
int rightPin = 14; //d5
int downPin = 0; //d3

// Replace with your network credentials
const char *ssid = "****";
const char *password = "****";

// Set web server port number to 80
ESP8266WebServer server ( 80 );

void handleRoot() {
    server.send(200, "text/html", "<html>\
  <head>\
    <meta http-equiv='refresh' content='1'/>\
    <title>ESP8266 Demo</title>\
    <style>\
      body { background-color: #cccccc; font-family: Arial, Helvetica, Sans-Serif; Color: #000088; }\
    </style>\
  </head>\
  <body>\
    <p>*R^</p>\
  </body>\
</html>");  
}

void handleNotFound(){
  server.send(404, "text/plain", "404: Not found"); // Send HTTP status 404 (Not Found) when there's no handler for the URI in the request
}


void setup() {
  Serial.begin ( 115200 );
  WiFi.mode ( WIFI_STA );
  WiFi.begin ( ssid, password );
  Serial.println ( "" );
  
  //wait for connection
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  // Print local IP address and start web server
  Serial.println("");
  Serial.print("Connected to ");
  Serial.println(ssid);
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
  
  if ( MDNS.begin ( "esp8266" ) ) {
    Serial.println ( "MDNS responder started" );
  }

  server.on ( "/", button );
  server.onNotFound ( handleNotFound );
  server.begin();
  Serial.println ( "HTTP server started" );
}

void loop(void){
  server.handleClient(); 
  button();
}


void button(){
  if (digitalRead(rightPin) == LOW)
  {
    server.send(200, "text/html", "<html>\
  <head>\
    <meta http-equiv='refresh' content='1'/>\
    <title>ESP8266 Demo</title>\
    <style>\
      body { background-color: #cccccc; font-family: Arial, Helvetica, Sans-Serif; Color: #000088; }\
    </style>\
  </head>\
  <body>\
    <p>*R^</p>\
  </body>\
</html>");
  }

  if (digitalRead(leftPin) == LOW)
  {
    server.send(200, "text/html", "<html>\
  <head>\
    <meta http-equiv='refresh' content='1'/>\
    <title>ESP8266 Demo</title>\
    <style>\
      body { background-color: #cccccc; font-family: Arial, Helvetica, Sans-Serif; Color: #000088; }\
    </style>\
  </head>\
  <body>\
    <p>*L^</p>\
  </body>\
</html>");
  
  }

  if (digitalRead(upPin) == LOW)
  {
    server.send(200, "text/html", "<html>\
  <head>\
    <meta http-equiv='refresh' content='1'/>\
    <title>ESP8266 Demo</title>\
    <style>\
      body { background-color: #cccccc; font-family: Arial, Helvetica, Sans-Serif; Color: #000088; }\
    </style>\
  </head>\
  <body>\
    <p>*U^</p>\
  </body>\
</html>");

  }


  if (digitalRead(downPin) == LOW)
  {
    server.send(200, "text/html", "<html>\
  <head>\
    <meta http-equiv='refresh' content='1'/>\
    <title>ESP8266 Demo</title>\
    <style>\
      body { background-color: #cccccc; font-family: Arial, Helvetica, Sans-Serif; Color: #000088; }\
    </style>\
  </head>\
  <body>\
    <p>*D^</p>\
  </body>\
</html>");
  
  } 

  if (digitalRead(centerPin) == LOW)
  {
    server.send(200, "text/html", "<html>\
  <head>\
    <meta http-equiv='refresh' content='1'/>\
    <title>ESP8266 Demo</title>\
    <style>\
      body { background-color: #cccccc; font-family: Arial, Helvetica, Sans-Serif; Color: #000088; }\
    </style>\
  </head>\
  <body>\
    <p>*C^</p>\
  </body>\
</html>");  
  }

  if (digitalRead(rightPin) == HIGH && digitalRead(leftPin) == HIGH && digitalRead(upPin) == HIGH && digitalRead(downPin) == HIGH && digitalRead(centerPin) == HIGH) {
      server.send(200, "text/html", "<html>\
        <head>\
          <meta http-equiv='refresh' content='1'/>\
          <title>ESP8266 Demo</title>\
          <style>\
            body { background-color: #cccccc; font-family: Arial, Helvetica, Sans-Serif; Color: #000088; }\
          </style>\
        </head>\
        <body>\
          <p>*E^</p>\
        </body>\
      </html>"); 
  }

}

