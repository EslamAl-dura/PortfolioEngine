using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Domain.Configs;

// Models/AdminIcon.cs
public record AdminIcon(string Key, string Label);

// Utilities/IconConfig.cs
public static class IconConfig
{
    public const string DefaultIcon = "question-circle";

    public static readonly List<AdminIcon> AllowedIcons = new()
{
    // --- Dashboard & Navigation ---
    new("speedometer2", "Dashboard / Overview"),
    new("house", "Home / Main Page"),
    new("grid-1x2", "Layout / Sections"),
    new("gear", "Settings"),
    new("sliders", "Preferences / Controls"),

    // --- Content & Portfolio ---
    new("briefcase", "Experience / Work"),
    new("folder2-open", "Portfolio Showcase"),
    new("terminal", "Skills / Tech Stack"),
    new("journal-text", "Blog / Articles"),
    new("award", "Certifications / Achievements"),
    new("mortarboard", "Education"),
    new("file-earmark-person", "Resume / CV"),
    new("chat-square-quote", "Testimonials / Reviews"),
    new("stars", "Featured Content / Highlights"),

    // --- Analytics & Communications ---
    new("graph-up-arrow", "Analytics / Traffic"),
    new("eye", "Views / Impressions"),
    new("envelope", "Messages / Contact Forms"),
    new("send", "Outbox / Newsletter"),
    new("bell", "Notifications / Alerts"),
    new("inbox", "Submissions"),

    // --- Developer & Tech Stack ---
    new("database", "Databases / Storage"),
    new("cpu", "Services / Backend"),
    new("layers", "Architecture / System Design"),
    new("box-seam", "Packages / Libraries"),
    new("cloud-arrow-up", "Deployments / DevOps"),
    new("git", "Version Control"),
    new("bug", "Issue Tracker / Debugging"),
    new("lightning-charge", "Performance / Speed"),

    // --- Social & External Links ---
    new("github", "GitHub"),
    new("linkedin", "LinkedIn"),
    new("twitter-x", "Twitter / X"),
    new("instagram", "Instagram"),
    new("youtube", "YouTube"),
    new("medium", "Medium"),
    new("discord", "Discord"),
    new("stack-overflow", "Stack Overflow"),
    new("globe", "Website / Live Demo"),
    new("link-45deg", "External Link"),

    // --- Commerce & Admin Tools ---
    new("cart", "Services Store / Pricing"),
    new("credit-card", "Payments / Invoices"),
    new("person", "User Profile"),
    new("shield-lock", "Security / Auth"),
    new("key", "API Keys / Access Tokens"),
    new("trash", "Archive / Cleanup"),

    // --- backend icons ---
    new("braces", "braces / backend"),

    // --- devops icons ---
    new("infinity", "infinity / devops"),
    new("box-seam", "box seam / container"),
    
    // --- frontend icons ---
    new("code-slash", "Projects / Source Code"),
};

    // Fast lookup HashSet for security validation
    private static readonly HashSet<string> ValidKeys = new(AllowedIcons.Select(i => i.Key));

    public static bool IsValid(string? key) => !string.IsNullOrEmpty(key) && ValidKeys.Contains(key);

    public static string GetSafeIcon(string? key) => IsValid(key) ? key! : DefaultIcon;
}
