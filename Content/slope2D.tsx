<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.4" tiledversion="1.4.3" name="slope2D" tilewidth="48" tileheight="48" tilecount="40" columns="8">
 <image source="Image/TileSet_Slope2D.png" trans="ffffff" width="384" height="240"/>
 <tile id="0" type="0"/>
 <tile id="1" type="2">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0">
    <polygon points="0,0 48,0 48,48 0,48"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="2" type="3">
  <properties>
   <property name="ClimbL" type="bool" value="false"/>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="2" x="0" y="48">
    <polygon points="0,0 48,-24 48,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="3" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="24">
    <polygon points="0,0 48,-24 48,24 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="4" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0">
    <polygon points="0,0 48,24 48,48 0,48"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="5" type="3">
  <properties>
   <property name="ClimbR" type="bool" value="false"/>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="24">
    <polygon points="0,0 48,24 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="6" type="2">
  <properties>
   <property name="ClimbL" type="bool" value="false"/>
   <property name="ClimbR" type="bool" value="false"/>
   <property name="passLevel" type="int" value="2"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0">
    <polygon points="0,0 48,0 48,48 0,48"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="7" type="2"/>
 <tile id="8" type="2">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="24">
    <polygon points="0,0 48,0 48,24 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="9" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="48">
    <polyline points="0,0 48,-48 48,0 0,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="10" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0">
    <polygon points="0,0 48,48 0,48"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="11" type="2">
  <properties>
   <property name="ClimbR" type="bool" value="true"/>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="0" y="0">
    <polygon points="0,0 24,0 24,48 0,48"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="12" type="2">
  <properties>
   <property name="ClimbL" type="bool" value="true"/>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="24" y="0">
    <polygon points="0,0 24,0 24,48 0,48"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="13" type="2">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0">
    <polygon points="0,0 48,0 48,24 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="14" type="3"/>
 <tile id="15" type="3"/>
 <tile id="16" type="3">
  <properties>
   <property name="ClimbL" type="bool" value="false"/>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="48">
    <polygon points="0,0 48,-12 48,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="17" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="36">
    <polygon points="0,0 48,-12 48,12 0,12"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="18" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="24">
    <polygon points="0,0 48,-12 48,24 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="19" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="12">
    <polygon points="0,0 48,-12 48,36 0,36"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="20" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0">
    <polygon points="0,0 48,12 48,48 0,48"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="21" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="12">
    <polygon points="0,0 48,12 48,36 0,36"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="22" type="3">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="24">
    <polygon points="0,0 48,12 48,24 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="23" type="3">
  <properties>
   <property name="ClimbR" type="bool" value="false"/>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="36">
    <polygon points="0,0 48,12 0,12"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="24" type="2">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0">
    <polygon points="0,0 24,0 24,24 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="25" type="2">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="24" y="0">
    <polygon points="0,0 24,0 24,24 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="26" type="2">
  <properties>
   <property name="ClimbL" type="bool" value="false"/>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="24" y="24">
    <polygon points="0,0 24,0 24,24 0,24"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="27" type="2">
  <properties>
   <property name="passLevel" type="int" value="3"/>
  </properties>
 </tile>
 <tile id="28" type="2">
  <properties>
   <property name="ClimbR" type="bool" value="false"/>
   <property name="passLevel" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="24">
    <polygon points="0,0 24,0 24,24 0,24 0,0"/>
   </object>
  </objectgroup>
 </tile>
</tileset>
