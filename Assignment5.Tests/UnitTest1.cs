using System.Collections.Generic;
using Xunit;
using Assignment5IFP;

namespace Assignment5.Tests;

public class WorkflowTests
{
    [Fact]
    public void NormalWorkflow_ValidDataset_ProducesExpectedResult()
    {
        var inputs = new List<DeliveryRequest?>
        {
            new("ORD-001", 5.0, false)
        };
        Func<double, double> policy = d => 500 + (100 * d);

        var valid = Program.Stage1_ValidateAndNormalize(inputs);
        var priced = Program.Stage2_CalculateCost(valid, policy, "Test");
        var summary = Program.Stage3_CreateSummary(priced);

        Assert.Single(summary);
        Assert.Equal("ORD-001", summary[0].OrderId);
        Assert.Equal(1000.0, summary[0].TotalCost);
    }

    [Fact]
    public void EmptyDataset_ValidEmptyInput_ProducesEmptyResult()
    {
        var inputs = new List<DeliveryRequest?>();
        Func<double, double> policy = d => 500 + (100 * d);

        var valid = Program.Stage1_ValidateAndNormalize(inputs);
        var priced = Program.Stage2_CalculateCost(valid, policy, "Test");
        var summary = Program.Stage3_CreateSummary(priced);

        Assert.Empty(summary);
    }

    [Fact]
    public void BoundaryValue_ExactThreshold_IsIncludedAndCalculated()
    {
        var inputs = new List<DeliveryRequest?>
        {
            new("ORD-BND", 0.0, false)
        };
        Func<double, double> policy = d => 500 + (100 * d);

        var valid = Program.Stage1_ValidateAndNormalize(inputs);
        var priced = Program.Stage2_CalculateCost(valid, policy, "Test");

        Assert.Single(valid);
        Assert.Equal(500.0, priced[0].TotalCost);
    }

    [Fact]
    public void InvalidAndMissingInput_MalformedAndNull_AreRejected()
    {
        var inputs = new List<DeliveryRequest?>
        {
            new("ORD-INV", -5.0, false),
            null,
            new("", 4.0, false)
        };

        var valid = Program.Stage1_ValidateAndNormalize(inputs);

        Assert.Empty(valid);
    }

    [Fact]
    public void TwoPolicies_SameInput_ProduceDifferentResults()
    {
        var inputs = new List<DeliveryRequest?>
        {
            new("ORD-001", 5.0, false)
        };
        Func<double, double> standard = d => 500 + (100 * d);
        Func<double, double> express = d => 800 + (150 * d);

        var valid = Program.Stage1_ValidateAndNormalize(inputs);
        var resStandard = Program.Stage2_CalculateCost(valid, standard, "Standard");
        var resExpress = Program.Stage2_CalculateCost(valid, express, "Express");

        Assert.Equal(1000.0, resStandard[0].TotalCost);
        Assert.Equal(1550.0, resExpress[0].TotalCost);
        Assert.NotEqual(resStandard[0].TotalCost, resExpress[0].TotalCost);
    }

    [Fact]
    public void PurityAndStages_InputCollectionRemainsUnchanged()
    {
        var inputs = new List<DeliveryRequest?>
        {
            new("ORD-001", 5.0, false)
        };
        int originalCount = inputs.Count;
        var originalItem = inputs[0];

        var valid = Program.Stage1_ValidateAndNormalize(inputs);

        Assert.Equal(originalCount, inputs.Count);
        Assert.Equal(originalItem, inputs[0]);
        Assert.Single(valid);
    }
}