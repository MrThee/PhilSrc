using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

public static class BezierUtils {

    // http://digerati-illuminatus.blogspot.com/2008/05/approximating-semicircle-with-cubic.html
    // NOTE: My approximation below prioritizes making sure the start/end tangents are parallel w/ each other
    // over making sure the radius of curvature stays constant. That's why we don't do the second step of
    // insetting p1 and p2 towards each other.
    public static BezierCurve3D FormHalfCircle(Vector3 circleCenter, float circleRadius, Vector3 arcStartRadialDirection, Vector3 arcAxis)
    {
        Vector3 arcEndRadialDirection = -(arcStartRadialDirection);

        Vector3 arcStart = circleCenter + (circleRadius * arcStartRadialDirection);
        Vector3 arcEnd = circleCenter + (circleRadius * arcEndRadialDirection);
        Vector3 startTangent = Vector3.Cross(arcAxis, arcStartRadialDirection).normalized;
        // "yValueOffset = radius * 4f / 3f"
        const float coeff = 1.3333333f;
        Vector3 offsetAlongTangent = (circleRadius * coeff) * startTangent;
        Vector3 p1 = arcStart + offsetAlongTangent;
        // Same direction for p2, against arcEnd
        Vector3 p2 = arcEnd + offsetAlongTangent;

        return new BezierCurve3D(arcStart, p1, p2, arcEnd);
    }

    public static BezierCurve3D Wrap180AroundUnboundCylinder(
        Vector3 arbitraryCylinderCenterAxisPoint, float cylinderRadius, Vector3 cylinderWrapAscentDir,
        Vector3 contactStart, Vector3 contactTangentDir)
    {
        Vector3 arbCenter2Contact = contactStart - arbitraryCylinderCenterAxisPoint;
        Vector3 centerStart2Contact = Vector3.ProjectOnPlane(arbCenter2Contact, cylinderWrapAscentDir);
        Vector3 cylinderStartCenter = contactStart - centerStart2Contact;

        // Start with a vanilla, planar half-circle
        var arc = FormHalfCircle(cylinderStartCenter, cylinderRadius, centerStart2Contact.normalized, cylinderWrapAscentDir);

        // move p1, p2, and p3 up along ascent direction
        // ty/tx = p1_ascentY/radius
        // p1_ascentY = radius * ty / tx;
        float ty = Vector3.Dot(contactTangentDir, cylinderWrapAscentDir);
        float tx = Vector3.Dot(contactTangentDir, (arc.p1 - arc.p0).normalized);
        Vector3 pointAscent = (cylinderRadius * ty / tx) * cylinderWrapAscentDir;
        arc.p1 += pointAscent;
        arc.p2 += pointAscent;
        arc.p3 += pointAscent;
        return arc;
    }

    public static BezierCurve3D Wrap180AroundConicSegment(
        Vector3 arbitraryConeAxisPoint, float startRadius, float endRadius, Vector3 coneWrapAscentDir, float segmentHeight,
        Vector3 contactStart
    ) {
        Vector3 arbCenter2Contact = contactStart - arbitraryConeAxisPoint;
        Vector3 centerStart2Contact = Vector3.ProjectOnPlane(arbCenter2Contact, coneWrapAscentDir);
        Vector3 coneStartCenter = contactStart - centerStart2Contact;

        Vector3 circleStartDir = centerStart2Contact.normalized;
        var halfCircleStart = FormHalfCircle(coneStartCenter, startRadius, circleStartDir, coneWrapAscentDir);
        Vector3 coneCenterEnd = coneStartCenter + (segmentHeight * coneWrapAscentDir);
        var halfCircleEnd = FormHalfCircle(coneCenterEnd, endRadius, circleStartDir, coneWrapAscentDir);

        const float oneThird = 0.333333333f;
        Vector3 thirdWayUp = (oneThird * segmentHeight) * coneWrapAscentDir;

        Vector3 p0 = halfCircleStart.p0;
        Vector3 p3 = halfCircleEnd.p3;
        Vector3 p1 = halfCircleStart.p1 + thirdWayUp;
        Vector3 p2 = halfCircleEnd.p2 - thirdWayUp;
        return new BezierCurve3D(p0, p1, p2, p3);
    }

    // https://mechanicalexpressions.com/explore/geometric-modeling/circle-spline-approximation.pdf
    // This paper has a proposal for approximating a quarter circle w/ a cubic spline
    // So if i wanted to approximate a half circle w/ more precision while keeping the start/end tangents
    // parallel, i could return a ValueTuple of 2 quarter-circle-approximating Bezier Curves.

}

}