using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;

namespace CarDynamics
{
    /// <summary>
    /// Wrapper class for base directinput device management and initializatation.
    /// 
    /// By Christopher M. Park -- http://www.christophermpark.com
    /// </summary>
    public sealed class DirectInputWrapper
    {
        #region ForceType
        /// <summary>
        /// This enumeration simply provides a shorthand way to reference
        /// the custom forces we create -- it's not a base part of DirectX,
        /// though, so you can use a completely different method.
        /// </summary>
        public enum ForceType
        {
            VeryBriefJolt,
            BriefJolt,
            LowRumble,
            HardRumble
        }
        #endregion

        //this class lets us send FF commands to up to two game pads,
        //but your own class could support as many as you want
        private static Device device1;
        private static Device device2;
        private static Form1 tempForm ;
        private static Dictionary<ForceType, EffectObject> P1Forces;
        private static Dictionary<ForceType, EffectObject> P2Forces;

        #region Initialize
        /// <summary>
        /// Initialize DirectInput
        /// </summary>
        public static void Initialize( System.Windows.Forms.Control Parent )
        {
            if ( device1 != null )
            {
                device1.Dispose();
                device1 = null;
            }

            if ( device2 != null )
            {
                device2.Dispose();
                device2 = null;
            }
            
            foreach ( DeviceInstance instance in Manager.GetDevices( DeviceClass.GameControl,
                EnumDevicesFlags.AttachedOnly ) )
            {
                if ( device1 == null )
                    device1 = new Device( instance.InstanceGuid );
                else if ( device2 == null )
                    device2 = new Device( instance.InstanceGuid );
            }

            DisposeForces();
            P1Forces = new Dictionary<ForceType, EffectObject>();
            P2Forces = new Dictionary<ForceType, EffectObject>();

            InitializeDevice( Parent, device1 );
            InitializeDevice( Parent, device2 );
        }
        #endregion

        #region InitializeDevice
        private static void InitializeDevice( System.Windows.Forms.Control Parent, Device Dev )
        {
            if ( Dev == null )
                return;

            Dev.SetDataFormat( DeviceDataFormat.Joystick );
            Dev.SetCooperativeLevel( Parent, CooperativeLevelFlags.Background |
                CooperativeLevelFlags.Exclusive );
            Dev.Properties.AxisModeAbsolute = true;
            //Dev.Properties.AutoCenter = false;
            Dev.Acquire();

            int[] axis = null;

            // Enumerate any axes
            foreach ( DeviceObjectInstance doi in Dev.Objects )
            {
                if ( ( doi.ObjectId & (int)DeviceObjectTypeFlags.Axis ) != 0 )
                {
                    // We found an axis, set the range to a max of 10,000
                    Dev.Properties.SetRange( ParameterHow.ById,
                        doi.ObjectId, new InputRange( -5000, 5000 ) );
                }

                int[] temp;

                // Get info about first two FF axii on the device
                if ( ( doi.Flags & (int)ObjectInstanceFlags.Actuator ) != 0 )
                {
                    if ( axis != null )
                    {
                        temp = new int[axis.Length + 1];
                        axis.CopyTo( temp, 0 );
                        axis = temp;
                    }
                    else
                    {
                        axis = new int[1];
                    }

                    // Store the offset of each axis.
                    axis[axis.Length - 1] = doi.Offset;
                    if ( axis.Length == 2 )
                    {
                        break;
                    }
                }
            }

            Dictionary<ForceType, EffectObject> forces;

            if ( Dev == device1 )
                forces = P1Forces;
            else
                forces = P2Forces;

            try
            {
                if ( axis != null )
                {
                    forces.Add( ForceType.VeryBriefJolt,
                        InitializeForce( Dev, EffectType.ConstantForce, axis,
                            6000, EffectFlags.ObjectOffsets | EffectFlags.Spherical, 150000 ) );
                    forces.Add( ForceType.BriefJolt,
                        InitializeForce( Dev, EffectType.ConstantForce, axis,
                            10000, EffectFlags.ObjectOffsets | EffectFlags.Spherical, 250000 ) );
                    forces.Add( ForceType.LowRumble,
                        InitializeForce( Dev, EffectType.ConstantForce, axis,
                            2000, EffectFlags.ObjectOffsets | EffectFlags.Cartesian, 900000 ) );
                    forces.Add( ForceType.HardRumble,
                        InitializeForce( Dev, EffectType.ConstantForce, axis,
                            10000, EffectFlags.ObjectOffsets | EffectFlags.Spherical, 2000000 ) );
                }
            }
            catch ( Exception e ) 
            {
                System.Windows.Forms.MessageBox.Show( "Could not initalize force feedback:\n\n" + e );
            }
        }
        #endregion

        #region DisposeForces
        public static void DisposeForces()
        {
            if ( P1Forces != null )
            {
                foreach ( EffectObject o in P1Forces.Values )
                    o.Dispose();
                P1Forces = null;
            }

            if ( P2Forces != null )
            {
                foreach ( EffectObject o in P2Forces.Values )
                    o.Dispose();
                P2Forces = null;
            }
        }
        #endregion

        #region SendForce
        public static void SendForce( ForceType Type, bool IsPlayer1 )
        {
            Dictionary<ForceType, EffectObject> forces;

            if ( IsPlayer1 )
                forces = P1Forces;
            else
                forces = P2Forces;

            if ( forces == null )
                return;
            if ( !forces.ContainsKey( Type ) )
                return;

            EffectObject force = forces[Type];
            force.Start( 1 );
        }
        #endregion

        #region InitializeForce
        public static EffectObject InitializeForce( Device Dev, EffectType Type,
            int[] Axis, int Magnitude, EffectFlags Flags, int Duration )
        {
            EffectObject eo = null;
            Effect e;

            foreach ( EffectInformation ei in Dev.GetEffects( EffectType.All ) )
            {
                if ( DInputHelper.GetTypeCode( ei.EffectType ) == (int)Type )
                {
                    e = new Effect();
                    e.SetDirection( new int[Axis.Length] );
                    e.SetAxes( new int[1] ); //this is the offending line in the Microsoft examples
                    //setting axes to 2 causes the dreaded "Value does not fall within expected range" error
                    //this is evidently a bug in Managed DirectX, at least affecting some game pads,
                    //and this is the only workaround I've found.
                    //I have not been able to successfully load FFE files in Managed DirectX
                    //due to this same problem (presumably, in the inner initialization code
                    //when loading from FFE, it is trying to load two axes there, as well).

                    //This problem exists with all verys of Managed DirectX, as far as I can tell,
                    //at least up through March 2008 when this example was written.

                    e.EffectType = Type;
                    e.ConditionStruct = new Condition[Axis.Length];
                    e.Duration = Duration;
                    e.Gain = 10000;
                    e.Constant = new ConstantForce();
                    e.Constant.Magnitude = Magnitude;
                    e.SamplePeriod = 0;
                    e.TriggerButton = (int)Microsoft.DirectX.DirectInput.Button.NoTrigger;
                    e.TriggerRepeatInterval = (int)DI.Infinite;
                    e.Flags = Flags;
                    e.UsesEnvelope = false;

                    // Create the effect, using the passed in guid.
                    eo = new EffectObject( ei.EffectGuid, e, Dev );
                }
            }

            return eo;
        }
        #endregion
    }
}
