using Content.Shared._RMC14.Attachable.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype; // Если нужно для LocId

namespace Content.Shared._RMC14.Attachable.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(AttachableToggleableSystem))]
public sealed partial class AttachableToggleablePreventShootComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool ShootWhenActive = true;

    [DataField, AutoNetworkedField]
    public string Message = "rmc-attachable-cannot-shoot"; 
}
