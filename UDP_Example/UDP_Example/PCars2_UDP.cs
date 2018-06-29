using System;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace PcarsUDP
{

    class PCars2_UDP
    {

        private UdpClient _listener;
        private IPEndPoint _groupEP;

        private UInt32 _PacketNumber;
        private UInt32 _CategoryPacketNumber;
        private byte _PartialPacketIndex;
        private byte _PartialPacketNumber;
        private byte _PacketType;
        private byte _PacketVersion;
        private sbyte _ViewedParticipantIndex;
        private byte _UnfilteredThrottle;
        private byte _UnfilteredBrake;
        private sbyte _UnfilteredSteering;
        private byte _UnfilteredClutch;
        private byte _CarFlags;
        private Int16 _OilTempCelsius;
        private UInt16 _OilPressureKPa;
        private Int16 _WaterTempCelsius;
        private UInt16 _WaterPressureKpa;
        private UInt16 _FuelPressureKpa;
        private byte _FuelCapacity;
        private byte _Brake;
        private byte _Throttle;
        private byte _Clutch;
        private float _FuelLevel;
        private float _Speed;
        private UInt16 _Rpm;
        private UInt16 _MaxRpm;
        private sbyte _Steering;
        private byte _GearNumGears;
        private byte _BoostAmount;
        private byte _CrashState;
        private float _OdometerKM;
        private float[] _Orientation = new float[3];
        private float[] _LocalVelocity = new float[3];
        private float[] _WorldVelocity = new float[3];
        private float[] _AngularVelocity = new float[3];
        private float[] _LocalAcceleration = new float[3];
        private float[] _WorldAcceleration = new float[3];
        private float[] _ExtentsCentre = new float[3];
        private byte[] _TyreFlags = new byte[4];
        private byte[] _Terrain = new byte[4];
        private float[] _TyreY = new float[4];
        private float[] _TyreRPS = new float[4];
        private byte[] _TyreTemp = new byte[4];
        private float[] _TyreHeightAboveGround = new float[4];
        private byte[] _TyreWear = new byte[4];
        private byte[] _BrakeDamage = new byte[4];
        private byte[] _SuspensionDamage = new byte[4];
        private Int16[] _BrakeTempCelsius = new Int16[4];
        private UInt16[] _TyreTreadTemp = new UInt16[4];
        private UInt16[] _TyreLayerTemp = new UInt16[4];
        private UInt16[] _TyreCarcassTemp = new UInt16[4];
        private UInt16[] _TyreRimTemp = new UInt16[4];
        private UInt16[] _TyreInternalAirTemp = new UInt16[4];
        private UInt16[] _TyreTempLeft = new UInt16[4];
        private UInt16[] _TyreTempCenter = new UInt16[4];
        private UInt16[] _TyreTempRight = new UInt16[4];
        private float[] _WheelLocalPositionY = new float[4];
        private float[] _RideHeight = new float[4];
        private float[] _SuspensionTravel = new float[4];
        private float[] _SuspensionVelocity = new float[4];
        private UInt16[] _SuspensionRideHeight = new UInt16[4];
        private UInt16[] _AirPressure = new UInt16[4];
        private float _EngineSpeed;
        private float _EngineTorque;
        private byte[] _Wings = new byte[2];

        //Timing
        private sbyte _NumberParticipants;
        private UInt32 _ParticipantsChangedTimestamp;
        private float _EventTimeRemaining;
        private float _SplitTimeAhead;
        private float _SplitTimeBehind;
        private float _SplitTime;
        private double[,] _ParticipantInfo = new double[32, 16];



        public PCars2_UDP(UdpClient listen, IPEndPoint group)
        {
            _listener = listen;
            _groupEP = group;
        }

        public void readPackets()
        {
            byte[] UDPpacket = listener.Receive(ref _groupEP);
            Stream stream = new MemoryStream(UDPpacket);
            var binaryReader = new BinaryReader(stream);

            ReadBaseUDP(stream, binaryReader);
            if (PacketType == 0)
            {
                ReadTelemetryData(stream, binaryReader);


            }
            else if (PacketType == 3)
            {
                ReadTimings(stream, binaryReader);
            }

        }

        public void ReadBaseUDP(Stream stream, BinaryReader binaryReader)
        {
            stream.Position = 0;
            PacketNumber = binaryReader.ReadUInt32();
            CategoryPacketNumber = binaryReader.ReadUInt32();
            PartialPacketIndex = binaryReader.ReadByte();
            PartialPacketNumber = binaryReader.ReadByte();
            PacketType = binaryReader.ReadByte();
            PacketVersion = binaryReader.ReadByte();
        }

        public void ReadTelemetryData(Stream stream, BinaryReader binaryReader)
        {
            stream.Position = 12;

            ViewedParticipantIndex = binaryReader.ReadSByte();
            UnfilteredThrottle = binaryReader.ReadByte();
            UnfilteredBrake = binaryReader.ReadByte();
            UnfilteredSteering = binaryReader.ReadSByte();
            UnfilteredClutch = binaryReader.ReadByte();
            CarFlags = binaryReader.ReadByte();
            OilTempCelsius = binaryReader.ReadInt16();
            OilPressureKPa = binaryReader.ReadUInt16();
            WaterTempCelsius = binaryReader.ReadInt16();
            WaterPressureKpa = binaryReader.ReadUInt16();
            FuelPressureKpa = binaryReader.ReadUInt16();
            FuelCapacity = binaryReader.ReadByte();
            Brake = binaryReader.ReadByte();
            Throttle = binaryReader.ReadByte();
            Clutch = binaryReader.ReadByte();
            FuelLevel = binaryReader.ReadSingle();
            Speed = binaryReader.ReadSingle();
            Rpm = binaryReader.ReadUInt16();
            MaxRpm = binaryReader.ReadUInt16();
            Steering = binaryReader.ReadSByte();
            GearNumGears = binaryReader.ReadByte();
            BoostAmount = binaryReader.ReadByte();
            CrashState = binaryReader.ReadByte();
            OdometerKM = binaryReader.ReadSingle();

            Orientation[0] = binaryReader.ReadSingle();
            Orientation[1] = binaryReader.ReadSingle();
            Orientation[2] = binaryReader.ReadSingle();

            LocalVelocity[0] = binaryReader.ReadSingle();
            LocalVelocity[1] = binaryReader.ReadSingle();
            LocalVelocity[2] = binaryReader.ReadSingle();

            WorldVelocity[0] = binaryReader.ReadSingle();
            WorldVelocity[1] = binaryReader.ReadSingle();
            WorldVelocity[2] = binaryReader.ReadSingle();

            AngularVelocity[0] = binaryReader.ReadSingle();
            AngularVelocity[1] = binaryReader.ReadSingle();
            AngularVelocity[2] = binaryReader.ReadSingle();

            LocalAcceleration[0] = binaryReader.ReadSingle();
            LocalAcceleration[1] = binaryReader.ReadSingle();
            LocalAcceleration[2] = binaryReader.ReadSingle();

            WorldAcceleration[0] = binaryReader.ReadSingle();
            WorldAcceleration[1] = binaryReader.ReadSingle();
            WorldAcceleration[2] = binaryReader.ReadSingle();

            ExtentsCentre[0] = binaryReader.ReadSingle();
            ExtentsCentre[1] = binaryReader.ReadSingle();
            ExtentsCentre[2] = binaryReader.ReadSingle();

            TyreFlags[0] = binaryReader.ReadByte();
            TyreFlags[1] = binaryReader.ReadByte();
            TyreFlags[2] = binaryReader.ReadByte();
            TyreFlags[3] = binaryReader.ReadByte();

            Terrain[0] = binaryReader.ReadByte();
            Terrain[1] = binaryReader.ReadByte();
            Terrain[2] = binaryReader.ReadByte();
            Terrain[3] = binaryReader.ReadByte();

            TyreY[0] = binaryReader.ReadSingle();
            TyreY[1] = binaryReader.ReadSingle();
            TyreY[2] = binaryReader.ReadSingle();
            TyreY[3] = binaryReader.ReadSingle();

            TyreRPS[0] = binaryReader.ReadSingle();
            TyreRPS[1] = binaryReader.ReadSingle();
            TyreRPS[2] = binaryReader.ReadSingle();
            TyreRPS[3] = binaryReader.ReadSingle();

            TyreTemp[0] = binaryReader.ReadByte();
            TyreTemp[1] = binaryReader.ReadByte();
            TyreTemp[2] = binaryReader.ReadByte();
            TyreTemp[3] = binaryReader.ReadByte();

            TyreHeightAboveGround[0] = binaryReader.ReadSingle();
            TyreHeightAboveGround[1] = binaryReader.ReadSingle();
            TyreHeightAboveGround[2] = binaryReader.ReadSingle();
            TyreHeightAboveGround[3] = binaryReader.ReadSingle();

            TyreWear[0] = binaryReader.ReadByte();
            TyreWear[1] = binaryReader.ReadByte();
            TyreWear[2] = binaryReader.ReadByte();
            TyreWear[3] = binaryReader.ReadByte();

            BrakeDamage[0] = binaryReader.ReadByte();
            BrakeDamage[1] = binaryReader.ReadByte();
            BrakeDamage[2] = binaryReader.ReadByte();
            BrakeDamage[3] = binaryReader.ReadByte();

            SuspensionDamage[0] = binaryReader.ReadByte();
            SuspensionDamage[1] = binaryReader.ReadByte();
            SuspensionDamage[2] = binaryReader.ReadByte();
            SuspensionDamage[3] = binaryReader.ReadByte();

            BrakeTempCelsius[0] = binaryReader.ReadInt16();
            BrakeTempCelsius[1] = binaryReader.ReadInt16();
            BrakeTempCelsius[2] = binaryReader.ReadInt16();
            BrakeTempCelsius[3] = binaryReader.ReadInt16();

            TyreTreadTemp[0] = binaryReader.ReadUInt16();
            TyreTreadTemp[1] = binaryReader.ReadUInt16();
            TyreTreadTemp[2] = binaryReader.ReadUInt16();
            TyreTreadTemp[3] = binaryReader.ReadUInt16();

            TyreLayerTemp[0] = binaryReader.ReadUInt16();
            TyreLayerTemp[1] = binaryReader.ReadUInt16();
            TyreLayerTemp[2] = binaryReader.ReadUInt16();
            TyreLayerTemp[3] = binaryReader.ReadUInt16();

            TyreCarcassTemp[0] = binaryReader.ReadUInt16();
            TyreCarcassTemp[1] = binaryReader.ReadUInt16();
            TyreCarcassTemp[2] = binaryReader.ReadUInt16();
            TyreCarcassTemp[3] = binaryReader.ReadUInt16();

            TyreRimTemp[0] = binaryReader.ReadUInt16();
            TyreRimTemp[1] = binaryReader.ReadUInt16();
            TyreRimTemp[2] = binaryReader.ReadUInt16();
            TyreRimTemp[3] = binaryReader.ReadUInt16();

            TyreInternalAirTemp[0] = binaryReader.ReadUInt16();
            TyreInternalAirTemp[1] = binaryReader.ReadUInt16();
            TyreInternalAirTemp[2] = binaryReader.ReadUInt16();
            TyreInternalAirTemp[3] = binaryReader.ReadUInt16();

            TyreTempLeft[0] = binaryReader.ReadUInt16();
            TyreTempLeft[1] = binaryReader.ReadUInt16();
            TyreTempLeft[2] = binaryReader.ReadUInt16();
            TyreTempLeft[3] = binaryReader.ReadUInt16();

            TyreTempCenter[0] = binaryReader.ReadUInt16();
            TyreTempCenter[1] = binaryReader.ReadUInt16();
            TyreTempCenter[2] = binaryReader.ReadUInt16();
            TyreTempCenter[3] = binaryReader.ReadUInt16();

            TyreTempRight[0] = binaryReader.ReadUInt16();
            TyreTempRight[1] = binaryReader.ReadUInt16();
            TyreTempRight[2] = binaryReader.ReadUInt16();
            TyreTempRight[3] = binaryReader.ReadUInt16();

            WheelLocalPositionY[0] = binaryReader.ReadSingle();
            WheelLocalPositionY[1] = binaryReader.ReadSingle();
            WheelLocalPositionY[2] = binaryReader.ReadSingle();
            WheelLocalPositionY[3] = binaryReader.ReadSingle();

            RideHeight[0] = binaryReader.ReadSingle();
            RideHeight[1] = binaryReader.ReadSingle();
            RideHeight[2] = binaryReader.ReadSingle();
            RideHeight[3] = binaryReader.ReadSingle();

            SuspensionTravel[0] = binaryReader.ReadSingle();
            SuspensionTravel[1] = binaryReader.ReadSingle();
            SuspensionTravel[2] = binaryReader.ReadSingle();
            SuspensionTravel[3] = binaryReader.ReadSingle();

            SuspensionVelocity[0] = binaryReader.ReadSingle();
            SuspensionVelocity[1] = binaryReader.ReadSingle();
            SuspensionVelocity[2] = binaryReader.ReadSingle();
            SuspensionVelocity[3] = binaryReader.ReadSingle();

            SuspensionRideHeight[0] = binaryReader.ReadUInt16();
            SuspensionRideHeight[1] = binaryReader.ReadUInt16();
            SuspensionRideHeight[2] = binaryReader.ReadUInt16();
            SuspensionRideHeight[3] = binaryReader.ReadUInt16();

            AirPressure[0] = binaryReader.ReadUInt16();
            AirPressure[1] = binaryReader.ReadUInt16();
            AirPressure[2] = binaryReader.ReadUInt16();
            AirPressure[3] = binaryReader.ReadUInt16();

            EngineSpeed = binaryReader.ReadSingle();
            EngineTorque = binaryReader.ReadSingle();

            Wings[0] = binaryReader.ReadByte();
            Wings[1] = binaryReader.ReadByte();
        }

        public void ReadTimings(Stream stream, BinaryReader binaryReader)
        {
            stream.Position = 12;
            NumberParticipants = binaryReader.ReadSByte();
            ParticipantsChangedTimestamp = binaryReader.ReadUInt32();
            EventTimeRemaining = binaryReader.ReadSingle();
            SplitTimeAhead = binaryReader.ReadSingle();
            SplitTimeBehind = binaryReader.ReadSingle();
            SplitTime = binaryReader.ReadSingle();

            for (int i = 0; i < 32; i++)
            {
                ParticipantInfo[i, 0] = Convert.ToDouble(binaryReader.ReadInt16());  //WorldPosition 
                ParticipantInfo[i, 1] = Convert.ToDouble(binaryReader.ReadInt16());  //WorldPosition
                ParticipantInfo[i, 2] = Convert.ToDouble(binaryReader.ReadInt16());  //WorldPosition
                ParticipantInfo[i, 3] = Convert.ToDouble(binaryReader.ReadInt16());  //Orientation
                ParticipantInfo[i, 4] = Convert.ToDouble(binaryReader.ReadInt16()); //Orientation 
                ParticipantInfo[i, 5] = Convert.ToDouble(binaryReader.ReadInt16());  //Orientation
                ParticipantInfo[i, 6] = Convert.ToDouble(binaryReader.ReadUInt16());  //sCurrentLapDistance
                ParticipantInfo[i, 7] = Convert.ToDouble(binaryReader.ReadByte()) - 128;  //sRacePosition
                byte Sector_ALL = binaryReader.ReadByte();
                var Sector_Extracted = Sector_ALL & 0x0F;
                ParticipantInfo[i, 8] = Convert.ToDouble(Sector_Extracted+1);   //sSector
                ParticipantInfo[i, 9] = Convert.ToDouble(binaryReader.ReadByte());  //sHighestFlag
                ParticipantInfo[i, 10] = Convert.ToDouble(binaryReader.ReadByte()); //sPitModeSchedule
                ParticipantInfo[i, 11] = Convert.ToDouble(binaryReader.ReadUInt16());//sCarIndex
                ParticipantInfo[i, 12] = Convert.ToDouble(binaryReader.ReadByte()); //sRaceState
                ParticipantInfo[i, 13] = Convert.ToDouble(binaryReader.ReadByte()); //sCurrentLap
                ParticipantInfo[i, 14] = Convert.ToDouble(binaryReader.ReadSingle()); //sCurrentTime
                ParticipantInfo[i, 15] = Convert.ToDouble(binaryReader.ReadSingle());  //sCurrentSectorTime
            }

        }

        public void close_UDP_Connection()
        {
            listener.Close();
        }

        public UdpClient listener
        {
            get
            {
                return _listener;
            }
            set
            {
                _listener = value;
            }
        }

        public IPEndPoint groupEP
        {
            get
            {
                return _groupEP;
            }
            set
            {
                _groupEP = value;
            }
        }

        public UInt32 PacketNumber
        {
            get
            {
                return _PacketNumber;
            }
            set
            {
                _PacketNumber = value;
            }
        }

        public UInt32 CategoryPacketNumber
        {
            get
            {
                return _CategoryPacketNumber;
            }
            set
            {
                _CategoryPacketNumber = value;
            }
        }

        public byte PartialPacketIndex
        {
            get
            {
                return _PartialPacketIndex;
            }
            set
            {
                _PartialPacketIndex = value;
            }
        }

        public byte PartialPacketNumber
        {
            get
            {
                return _PartialPacketNumber;
            }
            set
            {
                _PartialPacketNumber = value;
            }
        }

        public byte PacketType
        {
            get
            {
                return _PacketType;
            }
            set
            {
                _PacketType = value;
            }
        }

        public byte PacketVersion
        {
            get
            {
                return _PacketVersion;
            }
            set
            {
                _PacketVersion = value;
            }
        }

        public sbyte ViewedParticipantIndex
        {
            get
            {
                return _ViewedParticipantIndex;
            }
            set
            {
                _ViewedParticipantIndex = value;
            }
        }

        public byte UnfilteredThrottle
        {
            get
            {
                return _UnfilteredThrottle;
            }
            set
            {
                _UnfilteredThrottle = value;
            }
        }

        public byte UnfilteredBrake
        {
            get
            {
                return _UnfilteredBrake;
            }
            set
            {
                _UnfilteredBrake = value;
            }
        }

        public sbyte UnfilteredSteering
        {
            get
            {
                return _UnfilteredSteering;
            }
            set
            {
                _UnfilteredSteering = value;
            }
        }

        public byte UnfilteredClutch
        {
            get
            {
                return _UnfilteredClutch;
            }
            set
            {
                _UnfilteredClutch = value;
            }
        }

        public byte CarFlags
        {
            get
            {
                return _CarFlags;
            }
            set
            {
                _CarFlags = value;
            }
        }

        public Int16 OilTempCelsius
        {
            get
            {
                return _OilTempCelsius;
            }
            set
            {
                _OilTempCelsius = value;
            }
        }

        public UInt16 OilPressureKPa
        {
            get
            {
                return _OilPressureKPa;
            }
            set
            {
                _OilPressureKPa = value;
            }
        }

        public Int16 WaterTempCelsius
        {
            get
            {
                return _WaterTempCelsius;
            }
            set
            {
                _WaterTempCelsius = value;
            }
        }

        public UInt16 WaterPressureKpa
        {
            get
            {
                return _WaterPressureKpa;
            }
            set
            {
                _WaterPressureKpa = value;
            }
        }

        public UInt16 FuelPressureKpa
        {
            get
            {
                return _FuelPressureKpa;
            }
            set
            {
                _FuelPressureKpa = value;
            }
        }

        public byte FuelCapacity
        {
            get
            {
                return _FuelCapacity;
            }
            set
            {
                _FuelCapacity = value;
            }
        }

        public byte Brake
        {
            get
            {
                return _Brake;
            }
            set
            {
                _Brake = value;
            }
        }

        public byte Throttle
        {
            get
            {
                return _Throttle;
            }
            set
            {
                _Throttle = value;
            }
        }

        public byte Clutch
        {
            get
            {
                return _Clutch;
            }
            set
            {
                _Clutch = value;
            }
        }

        public float FuelLevel
        {
            get
            {
                return _FuelLevel;
            }
            set
            {
                _FuelLevel = value;
            }
        }

        public float Speed
        {
            get
            {
                return _Speed;
            }
            set
            {
                _Speed = value;
            }
        }

        public UInt16 Rpm
        {
            get
            {
                return _Rpm;
            }
            set
            {
                _Rpm = value;
            }
        }

        public UInt16 MaxRpm
        {
            get
            {
                return _MaxRpm;
            }
            set
            {
                _MaxRpm = value;
            }
        }

        public sbyte Steering
        {
            get
            {
                return _Steering;
            }
            set
            {
                _Steering = value;
            }
        }

        public byte GearNumGears
        {
            get
            {
                return _GearNumGears;
            }
            set
            {
                _GearNumGears = value;
            }
        }

        public byte BoostAmount
        {
            get
            {
                return _BoostAmount;
            }
            set
            {
                _BoostAmount = value;
            }
        }

        public byte CrashState
        {
            get
            {
                return _CrashState;
            }
            set
            {
                _CrashState = value;
            }
        }

        public float OdometerKM
        {
            get
            {
                return _OdometerKM;
            }
            set
            {
                _OdometerKM = value;
            }
        }

        public float[] Orientation
        {
            get
            {
                return _Orientation;
            }
            set
            {
                _Orientation = value;
            }
        }

        public float[] LocalVelocity
        {
            get
            {
                return _LocalVelocity;
            }
            set
            {
                _LocalVelocity = value;
            }
        }

        public float[] WorldVelocity
        {
            get
            {
                return _WorldVelocity;
            }
            set
            {
                _WorldVelocity = value;
            }
        }

        public float[] AngularVelocity
        {
            get
            {
                return _AngularVelocity;
            }
            set
            {
                _AngularVelocity = value;
            }
        }

        public float[] LocalAcceleration
        {
            get
            {
                return _LocalAcceleration;
            }
            set
            {
                _LocalAcceleration = value;
            }
        }

        public float[] WorldAcceleration
        {
            get
            {
                return _WorldAcceleration;
            }
            set
            {
                _WorldAcceleration = value;
            }
        }

        public float[] ExtentsCentre
        {
            get
            {
                return _ExtentsCentre;
            }
            set
            {
                _ExtentsCentre = value;
            }
        }

        public byte[] TyreFlags
        {
            get
            {
                return _TyreFlags;
            }
            set
            {
                _TyreFlags = value;
            }
        }

        public byte[] Terrain
        {
            get
            {
                return _Terrain;
            }
            set
            {
                _Terrain = value;
            }
        }

        public float[] TyreY
        {
            get
            {
                return _TyreY;
            }
            set
            {
                _TyreY = value;
            }
        }

        public float[] TyreRPS
        {
            get
            {
                return _TyreRPS;
            }
            set
            {
                _TyreRPS = value;
            }
        }

        public byte[] TyreTemp
        {
            get
            {
                return _TyreTemp;
            }
            set
            {
                _TyreTemp = value;
            }
        }

        public float[] TyreHeightAboveGround
        {
            get
            {
                return _TyreHeightAboveGround;
            }
            set
            {
                _TyreHeightAboveGround = value;
            }
        }

        public byte[] TyreWear
        {
            get
            {
                return _TyreWear;
            }
            set
            {
                _TyreWear = value;
            }
        }

        public byte[] BrakeDamage
        {
            get
            {
                return _BrakeDamage;
            }
            set
            {
                _BrakeDamage = value;
            }
        }

        public byte[] SuspensionDamage
        {
            get
            {
                return _SuspensionDamage;
            }
            set
            {
                _SuspensionDamage = value;
            }
        }

        public Int16[] BrakeTempCelsius
        {
            get
            {
                return _BrakeTempCelsius;
            }
            set
            {
                _BrakeTempCelsius = value;
            }
        }

        public UInt16[] TyreTreadTemp
        {
            get
            {
                return _TyreTreadTemp;
            }
            set
            {
                _TyreTreadTemp = value;
            }
        }

        public UInt16[] TyreLayerTemp
        {
            get
            {
                return _TyreLayerTemp;
            }
            set
            {
                _TyreLayerTemp = value;
            }
        }

        public UInt16[] TyreCarcassTemp
        {
            get
            {
                return _TyreCarcassTemp;
            }
            set
            {
                _TyreCarcassTemp = value;
            }
        }

        public UInt16[] TyreRimTemp
        {
            get
            {
                return _TyreRimTemp;
            }
            set
            {
                _TyreRimTemp = value;
            }
        }

        public UInt16[] TyreInternalAirTemp
        {
            get
            {
                return _TyreInternalAirTemp;
            }
            set
            {
                _TyreInternalAirTemp = value;
            }
        }

        public UInt16[] TyreTempLeft
        {
            get
            {
                return _TyreTempLeft;
            }
            set
            {
                _TyreTempLeft = value;
            }
        }

        public UInt16[] TyreTempCenter
        {
            get
            {
                return _TyreTempCenter;
            }
            set
            {
                _TyreTempCenter = value;
            }
        }

        public UInt16[] TyreTempRight
        {
            get
            {
                return _TyreTempRight;
            }
            set
            {
                _TyreTempRight = value;
            }
        }

        public float[] WheelLocalPositionY
        {
            get
            {
                return _WheelLocalPositionY;
            }
            set
            {
                _WheelLocalPositionY = value;
            }
        }

        public float[] RideHeight
        {
            get
            {
                return _RideHeight;
            }
            set
            {
                _RideHeight = value;
            }
        }

        public float[] SuspensionTravel
        {
            get
            {
                return _SuspensionTravel;
            }
            set
            {
                _SuspensionTravel = value;
            }
        }

        public float[] SuspensionVelocity
        {
            get
            {
                return _SuspensionVelocity;
            }
            set
            {
                _SuspensionVelocity = value;
            }
        }

        public UInt16[] SuspensionRideHeight
        {
            get
            {
                return _SuspensionRideHeight;
            }
            set
            {
                _SuspensionRideHeight = value;
            }
        }

        public UInt16[] AirPressure
        {
            get
            {
                return _AirPressure;
            }
            set
            {
                _AirPressure = value;
            }
        }

        public float EngineSpeed
        {
            get
            {
                return _EngineSpeed;
            }
            set
            {
                _EngineSpeed = value;
            }
        }

        public float EngineTorque
        {
            get
            {
                return _EngineTorque;
            }
            set
            {
                _EngineTorque = value;
            }
        }

        public byte[] Wings
        {
            get
            {
                return _Wings;
            }
            set
            {
                _Wings = value;
            }
        }

        public sbyte NumberParticipants
        {
            get
            {
                return _NumberParticipants;
            }
            set
            {
                _NumberParticipants = value;
            }
        }

        public UInt32 ParticipantsChangedTimestamp
        {
            get
            {
                return _ParticipantsChangedTimestamp;
            }
            set
            {
                _ParticipantsChangedTimestamp = value;
            }
        }

        public float EventTimeRemaining
        {
            get
            {
                return _EventTimeRemaining;
            }
            set
            {
                _EventTimeRemaining = value;
            }
        }

        public float SplitTimeAhead
        {
            get
            {
                return _SplitTimeAhead;
            }
            set
            {
                _SplitTimeAhead = value;
            }
        }

        public float SplitTimeBehind
        {
            get
            {
                return _SplitTimeBehind;
            }
            set
            {
                _SplitTimeBehind = value;
            }
        }

        public float SplitTime
        {
            get
            {
                return _SplitTime;
            }
            set
            {
                _SplitTime = value;
            }
        }

        public double[,] ParticipantInfo
        {
            get
            {
                return _ParticipantInfo;
            }
            set
            {
                _ParticipantInfo = value;
            }
        }
    }
}
