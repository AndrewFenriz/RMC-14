using System.Linq;
using Content.Client.Message;
using Content.Shared._RMC14.CCVar;
using Content.Shared._RMC14.Power;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Configuration;

namespace Content.Client._RMC14.Power;

[UsedImplicitly]
public sealed class RMCApcBui(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [Dependency] private readonly IConfigurationManager _config = default!;

    private static readonly Color BlueBackgroundColor = Color.FromHex("#3E6189");
    private static readonly Color GreenBackgroundColor = Color.FromHex("#1B9638");
    private static readonly Color GreenColor = Color.FromHex("#5AC229");
    private static readonly Color OrangeColor = Color.FromHex("#C99A29");
    private static readonly Color RedColor = Color.FromHex("#CE3E31");

    [ViewVariables]
    private RMCApcWindow? _window;

    protected override void Open()
    {
        base.Open();
        _window = this.CreateWindow<RMCApcWindow>();

        _window.CoverButton.OnPressed += _ => SendPredictedMessage(new RMCApcCoverBuiMsg());

        foreach (var channel in Enum.GetValues<RMCPowerChannel>())
        {
            var row = new RMCApcChannelRow();
            row.Label.SetMarkupPermissive(Loc.GetString("rmc-apc-ui-channel-label", ("channel", channel.ToString())));
            _window.Channels.AddChild(row);
        }

        Refresh();
    }

    public void Refresh()
    {
        if (_window is not { IsOpen: true })
            return;

        if (!EntMan.TryGetComponent(Owner, out RMCApcComponent? apc))
            return;

        var lockedMsg = apc.Locked
            ? Loc.GetString("rmc-apc-ui-lock-msg-unlock")
            : Loc.GetString("rmc-apc-ui-lock-msg-lock");
        _window.LockedLabel.SetMarkupPermissive(lockedMsg);

        _window.PowerStatusLabel.SetMarkupPermissive(Header(Loc.GetString("rmc-apc-ui-header-status")));
        _window.PowerChannelsLabel.SetMarkupPermissive(Header(Loc.GetString("rmc-apc-ui-header-channels")));
        _window.MiscLabel.SetMarkupPermissive(Header(Loc.GetString("rmc-apc-ui-header-misc")));

        _window.MainBreakerButton.Text = apc.MainBreakerButton ? Loc.GetString("rmc-apc-ui-on") : Loc.GetString("rmc-apc-ui-off");
        if (apc.MainBreakerButton)
        {
            _window.MainBreakerButton.Text = Loc.GetString("rmc-apc-ui-on");
            _window.MainBreakerButton.Pressed = true;
        }
        else
        {
            _window.MainBreakerButton.Text = Loc.GetString("rmc-apc-ui-off");
            _window.MainBreakerButton.Pressed = false;
        }

        _window.MainBreakerStatus.SetMarkupPermissive(apc.ExternalPower
            ? Green(Loc.GetString("rmc-apc-ui-ext-power-on"))
            : Red(Loc.GetString("rmc-apc-ui-ext-power-off"))
        );

        _window.PowerBar.MinValue = 0;
        _window.PowerBar.MaxValue = 1;
        _window.PowerBar.Value = apc.ChargePercentage;
        _window.PowerBarLabel.Text = Loc.GetString("rmc-apc-ui-charge-percent", ("percent", apc.ChargePercentage * 100));

        var chargeMode = apc.ChargeStatus switch
        {
            RMCApcChargeStatus.NotCharging => Red(Loc.GetString("rmc-apc-ui-charge-not-charging")),
            RMCApcChargeStatus.Charging => Orange(Loc.GetString("rmc-apc-ui-charge-charging")),
            RMCApcChargeStatus.FullCharge => Green(Loc.GetString("rmc-apc-ui-charge-full")),
            _ => throw new ArgumentOutOfRangeException(),
        };

        _window.ChargeMode.SetMarkupPermissive(chargeMode);
        _window.ChargeModeButton.Text = apc.ChargeModeButton ? Loc.GetString("rmc-apc-ui-auto") : Loc.GetString("rmc-apc-ui-off");

        foreach (int channel in Enum.GetValues<RMCPowerChannel>())
        {
            var row = (RMCApcChannelRow) _window.Channels.GetChild(channel);
            SetButtons(row, apc.Channels[channel]);
            row.Auto.OnPressed += _ => SendPredictedMessage(new RMCApcSetChannelBuiMsg((RMCPowerChannel) channel, RMCApcButtonState.Auto));
            // row.Off.OnPressed += _ => SendPredictedMessage(new RMCApcSetChannelBuiMsg((RMCPowerChannel) channel, RMCApcButtonState.Off));
            row.Off.Visible = false;
        }

        var multiplier = _config.GetCVar(RMCCVars.RMCPowerLoadMultiplier);
        var totalWatts = apc.Channels.Sum(c => c.Watts);
        _window.TotalLoadWatts.SetMarkupPermissive(Loc.GetString("rmc-apc-ui-total-load", ("watts", (int)(totalWatts / multiplier))));

        _window.CoverButton.Text = apc.CoverLockedButton ? Loc.GetString("rmc-apc-ui-cover-engaged") : Loc.GetString("rmc-apc-ui-cover-disengaged");
        _window.CoverButton.Disabled = apc.Locked;
    }

    private string Header(string header)
    {
        return $"[bold]{header}[/bold]";
    }

    private string Green(string str)
    {
        return $"[color={GreenColor.ToHex()}]{str}[/color]";
    }

    private string Orange(string str)
    {
        return $"[color={OrangeColor.ToHex()}]{str}[/color]";
    }

    private string Red(string str)
    {
        return $"[color={RedColor.ToHex()}]{str}[/color]";
    }

    private void SetButtons(RMCApcChannelRow row, RMCApcChannel channel)
    {
        var multiplier = _config.GetCVar(RMCCVars.RMCPowerLoadMultiplier);
        row.Auto.Pressed = channel.Button == RMCApcButtonState.Auto;
        row.On.Pressed = channel.Button == RMCApcButtonState.On;
        row.On.Visible = false; // TODO RMC14
        row.Off.Pressed = channel.Button == RMCApcButtonState.Off;
        row.Watts.SetMarkupPermissive(Loc.GetString("rmc-apc-ui-watts", ("watts", (int)(channel.Watts / multiplier))));
        row.Status.SetMarkupPermissive(channel.On ? $"{Green(Loc.GetString("rmc-apc-ui-on"))}" : $"{Red(Loc.GetString("rmc-apc-ui-off"))}");
    }
}
