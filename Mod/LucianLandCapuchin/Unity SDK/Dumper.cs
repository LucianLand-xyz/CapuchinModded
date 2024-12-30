#region using shit
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
#endregion

public class Dumper
{
    public static List<MethodInfo> DumpMethods(string component, string className)
    {
        List<MethodInfo> methodsToReturn = new List<MethodInfo>();
        GameObject componentObj = GameObject.Find(component);

        if (componentObj == null)
        {
            return methodsToReturn;
        }

        int pos = className.IndexOf("::");
        if (pos >= 0)
        {
            className = className.Substring(pos + 2); 
        }

        Component ccc = componentObj.GetComponent(className);
        if (ccc == null)
        {
            return methodsToReturn;
        }

        methodsToReturn.AddRange(ccc.GetType().GetMethods());

        return methodsToReturn;
    }

    public static List<string> DumpMethodsString(string component, string className)
    {
        List<string> methodsToReturn = new List<string>();
        Console.WriteLine($"\nDumping methods of {className}...\n");

        List<MethodInfo> methods = DumpMethods(component, className);

        foreach (var method in methods)
        {
            if (method == null)
            {
                continue;
            }

            string name = method.Name;
            methodsToReturn.Add(name);
            Console.WriteLine($"--> {name}\n");
        }

        return methodsToReturn;
    }

    public static List<Component> DumpClasses(string component)
    {
        List<Component> classesToReturn = new List<Component>();

        GameObject componentObj = GameObject.Find(component);
        if (componentObj == null)
        {
            return classesToReturn;
        }

        Component[] components = componentObj.GetComponents<Component>();

        foreach (var comp in components)
        {
            if (comp != null)
            {
                classesToReturn.Add(comp);
            }
        }

        return classesToReturn;
    }

    public static List<string> DumpClassesString(string component)
    {
        List<string> classesToReturn = new List<string>();
        Console.WriteLine($"\nDumping classes of {component}...\n");

        List<Component> classes = DumpClasses(component);
        foreach (var classObj in classes)
        {
            if (classObj == null)
                continue;

            string name = classObj.GetType().Namespace + "::" + classObj.GetType().Name;
            classesToReturn.Add(name);
            Console.WriteLine($"--> {name}\n");
        }

        return classesToReturn;
    }

    public static List<Component> DumpComponents()
    {
        List<Component> componentsToReturn = new List<Component>();

        Component[] components = Object.FindObjectsOfType<Component>();

        foreach (var comp in components)
        {
            if (comp != null)
            {
                componentsToReturn.Add(comp);
            }
        }

        return componentsToReturn;
    }

    public static List<string> DumpComponentsString()
    {
        List<string> componentsToReturn = new List<string>();
        Console.WriteLine("\nDumping components...\n");

        List<Component> components = DumpComponents();
        foreach (var component in components)
        {
            if (component == null)
                continue;

            GameObject obj = component.gameObject;
            string name = obj.name;
            componentsToReturn.Add(name);
        }

        return componentsToReturn;
    }
}
