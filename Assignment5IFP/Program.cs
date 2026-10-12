using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment5IFP;

public record DeliveryRequest(string OrderId, double DistanceKm, bool IsExpress);

public record ValidatedRequest(string OrderId, double DistanceKm, bool IsExpress);

public record PricedQuote(string OrderId, double DistanceKm, double TotalCost, string AppliedPolicy);

public class Program
{
    public static void Main(string[] args)
    {
        var rawRequests = new List<DeliveryRequest?>
        {
            new("ORD-001", 5.0, false),
            new("ORD-002", 2.0, true),
            new("ORD-003", -1.5, false),
            new("ORD-004", 10.0, true),
            null
        };

        Func<double, double> standardPolicy = distance => 500 + (100 * distance);
        Func<double, double> expressPolicy = distance => 800 + (150 * distance);

        Console.WriteLine("--- Running with Standard Policy ---");
        RunWorkflow(rawRequests, standardPolicy, "Standard Policy");

        Console.WriteLine("\n--- Running with Express Policy ---");
        RunWorkflow(rawRequests, expressPolicy, "Express Policy");
    }

    public static void RunWorkflow(IEnumerable<DeliveryRequest?> inputs, Func<double, double> pricingPolicy, string policyName)
    {
        var validRequests = Stage1_ValidateAndNormalize(inputs);
        var pricedQuotes = Stage2_CalculateCost(validRequests, pricingPolicy, policyName);
        var finalSummary = Stage3_CreateSummary(pricedQuotes);

        foreach (var quote in finalSummary)
        {
            Console.WriteLine($"Order: {quote.OrderId} | Distance: {quote.DistanceKm}km | Cost: {quote.TotalCost} KZT | Policy: {quote.AppliedPolicy}");
        }
    }

    public static IReadOnlyList<ValidatedRequest> Stage1_ValidateAndNormalize(IEnumerable<DeliveryRequest?> inputs)
    {
        var results = new List<ValidatedRequest>();
        foreach (var input in inputs)
        {
            if (input is not null && input.DistanceKm >= 0 && !string.IsNullOrWhiteSpace(input.OrderId))
            {
                results.Add(new ValidatedRequest(input.OrderId, input.DistanceKm, input.IsExpress));
            }
        }
        return results.AsReadOnly();
    }

    public static IReadOnlyList<PricedQuote> Stage2_CalculateCost(
        IEnumerable<ValidatedRequest> requests, 
        Func<double, double> policy, 
        string policyName)
    {
        var results = new List<PricedQuote>();
        foreach (var req in requests)
        {
            double cost = policy(req.DistanceKm);
            results.Add(new PricedQuote(req.OrderId, req.DistanceKm, cost, policyName));
        }
        return results.AsReadOnly();
    }

    public static IReadOnlyList<PricedQuote> Stage3_CreateSummary(IEnumerable<PricedQuote> quotes)
    {
        return quotes.OrderBy(q => q.TotalCost).ToList().AsReadOnly();
    }
}