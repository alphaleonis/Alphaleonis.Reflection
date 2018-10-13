---
uid: Alphaleonis.Reflection.Context.CustomReflectionContextBase
remarks: *content
---


@Alphaleonis.Reflection.Context.CustomReflectionContextBase provides a way to add or remove custom attributes
from reflection objects, or add dummy properties to those objects, without re-implementing
the complete reflection model.

This class has many similarities to [`System.Reflection.Context.CustomReflectionContext`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.context.customreflectioncontext), but it does have a few differences;

* It prevents a type being mapped multiple times, which may cause problems in many scenarios. This is done by   
  adding a @Alphaleonis.Reflection.Context.CustomReflectionContextIdAttribute with a unique ID for each reflection context in which 
  the type or member is mapped. If a member that has this attribute with the same ID as the reflection context is 
  sent to a `Map` method, it will simply be returned as is.

* It provides access to the projectors used to wrap each type or member, and allows specific overriding of only  
  those in which you are interested.  Also, the projectors by default delegate *all* methods to their base class, 
  while the ones in `System.Reflection.Contxet` throws a @System.NotImplementedException for many methods (at the
  time of writing).

* No convenience methods for adding additional properties to an object is currently provided by this class
  as is the case with the implementation in `System.Reflection.Context`.