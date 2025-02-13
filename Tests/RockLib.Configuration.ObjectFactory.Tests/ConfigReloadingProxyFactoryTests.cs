﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using RockLib.Configuration.ObjectFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Tests
{
   public class ConfigReloadingProxyFactoryTests
   {
      [Fact]
      public void MissingConstructorParametersAreSuppliedByTheResolver()
      {
         var config = new ConfigurationBuilder()
             .AddInMemoryCollection(new Dictionary<string, string> { { "waldo", "123" } })
             .Build();

         var garply = new Garply();

         var defaultTypes = new DefaultTypes().Add(typeof(IGrault), typeof(Grault));
         var resolver = new Resolver(t => garply, t => t == typeof(IGarply));

         var grault = config.CreateReloadingProxy<IGrault>(defaultTypes, resolver: resolver);

         Assert.Same(garply, grault.Garply);
         Assert.Equal(123, grault.Waldo);

         ChangeConfig(config, new KeyValuePair<string, string>("waldo", "456"));

         Assert.Same(garply, grault.Garply);
         Assert.Equal(456, grault.Waldo);
      }

      [Fact]
      public void NullConfigurationThrowsArgumentNullException()
      {
         IConfiguration? configuration = null;
         var interfaceType = typeof(IFoo);

         Assert.Throws<ArgumentNullException>(() => configuration!.CreateReloadingProxy(interfaceType));
      }

      [Fact]
      public void NullInterfaceTypeThrowsArgumentNullException()
      {
         var configuration = GetConfig();
         Type interfaceType = null!;

         Assert.Throws<ArgumentNullException>(() => configuration.CreateReloadingProxy(interfaceType));
      }

      [Fact]
      public void NonInterfaceInterfaceTypeThrowsArgumentException()
      {
         var configuration = GetConfig();
         var interfaceType = typeof(int);

         Assert.Throws<ArgumentException>(() => configuration.CreateReloadingProxy(interfaceType));
      }

      [Fact]
      public void EnumerableInterfaceTypeThrowsArgumentException()
      {
         var configuration = GetConfig();
         var interfaceType = typeof(IEnumerable<int>);

         Assert.Throws<ArgumentException>(() => configuration.CreateReloadingProxy(interfaceType));
      }

      [Fact]
      public void NullConfigurationThrowsArgumentNullExceptionGeneric()
      {
         IConfiguration? configuration = null;

         Assert.Throws<ArgumentNullException>(() => configuration!.CreateReloadingProxy<IFoo>());
      }

      [Fact]
      public void NonInterfaceInterfaceTypeThrowsArgumentExceptionGeneric()
      {
         var configuration = GetConfig();

         Assert.Throws<ArgumentException>(() => configuration.CreateReloadingProxy<int>());
      }

      [Fact]
      public void EnumerableInterfaceTypeThrowsArgumentExceptionGeneric()
      {
         var configuration = GetConfig();

         Assert.Throws<ArgumentException>(() => configuration.CreateReloadingProxy<IEnumerable<int>>());
      }

      [Fact]
      public void PropertiesWork()
      {
         var configuration = GetConfig();

         var foo = configuration.GetSection("foo").CreateReloadingProxy<IFoo>();

         Assert.Equal(123, foo.Bar);

         ChangeConfig(configuration);

         Assert.Equal(456, foo.Bar);
      }

      [Fact]
      public void MethodsWork()
      {
         var configuration = GetConfig();

         var foo = configuration.GetSection("foo").CreateReloadingProxy<IFoo>();

         Assert.Equal(123 * 2, foo.Baz());

         ChangeConfig(configuration);

         Assert.Equal(456 * 2, foo.Baz());
      }

      [Fact]
      public void EventsWork()
      {
         var configuration = GetConfig();

         var bar = configuration.GetSection("bar").CreateReloadingProxy<IBar>();

         var qux = -1;

         bar.Baz += (s, e) =>
         {
            qux = bar.Qux;
         };

         ((Bar)((ConfigReloadingProxy<IBar>)bar).Object).OnBaz();

         Assert.Equal(5, qux);

         ChangeConfig(configuration);

         qux = -1;

         ((Bar)((ConfigReloadingProxy<IBar>)bar).Object).OnBaz();

         Assert.Equal(10, qux);
      }

      [Fact]
      public void DisposableImplementationsAreDisposed()
      {
         var configuration = GetConfig();

         var baz = configuration.GetSection("baz").CreateReloadingProxy<IBaz>();

         var initialBaz = (Baz)((ConfigReloadingProxy<IBaz>)baz).Object;

         Assert.False(initialBaz.IsDisposed);

         ChangeConfig(configuration);

         var changedBaz = (Baz)((ConfigReloadingProxy<IBaz>)baz).Object;

         Assert.False(changedBaz.IsDisposed);
         Assert.True(initialBaz.IsDisposed);

         ((IDisposable)baz).Dispose();

         Assert.True(changedBaz.IsDisposed);
      }

      [Fact]
      public void ReadWriteReferenceTypePropertiesCopyTheOldValueWhenTheValueIsNotSpecifiedInChangedConfig()
      {
         var configuration = GetConfig();

         var foo = configuration.GetSection("foo").CreateReloadingProxy<IFoo>();
         foo.Qux = "abc";

         ChangeConfig(configuration);

         Assert.Equal("abc", foo.Qux);
      }

      [Fact]
      public void ReadWriteReferenceTypePropertiesDoNotCopyTheOldValueWhenTheValueIsSpecifiedInChangedConfig()
      {
         var configuration = GetConfig();

         var foo = configuration.GetSection("foo").CreateReloadingProxy<IFoo>();
         foo.Qux = "abc";

         ChangeConfig(configuration, new KeyValuePair<string, string>("foo:value:qux", "xyz"));

         Assert.Equal("xyz", foo.Qux);
      }

      [Fact]
      public void SettingReloadOnChangeToFalseCausesTheProxyToStopReloading()
      {
         var configuration = GetConfig();

         var foo = configuration.GetSection("foo").CreateReloadingProxy<IFoo>();

         ChangeConfig(configuration, new KeyValuePair<string, string>("foo:reloadOnChange", "false"));

         Assert.Equal(123, foo.Bar);
      }

      [Fact]
      public void ReloadMethodForcesTheUnderlyingObjectToReload()
      {
         var configuration = GetConfig();

         var foo = (ConfigReloadingProxy<IFoo>)configuration.GetSection("foo").CreateReloadingProxy<IFoo>();

         var initialObject = foo.Object;

         foo.Reload();

         var reloadedObject = foo.Object;

         Assert.NotSame(initialObject, reloadedObject);
      }

      [Fact]
      public void AnInitialReloadOnChangeOfFalseDoesNotCreateProxy()
      {
         var configuration = GetConfig(new KeyValuePair<string, string>("foo:reloadOnChange", "false"));

         var foo = configuration.GetSection("foo").CreateReloadingProxy<IFoo>();

         Assert.IsType<Foo>(foo);
      }

      [Fact]
      public void UnrelatedConfigChangeDoesNotCauseReload()
      {
         var configuration = GetConfig(new KeyValuePair<string, string>("garply", "abc"));

         var foo = (ConfigReloadingProxy<IFoo>)configuration.GetSection("foo").CreateReloadingProxy<IFoo>();

         var initialObject = foo.Object;

         ChangeConfig(configuration, settings: new Dictionary<string, string> { ["garply"] = "xyz" });

         var reloadedObject = foo.Object;

         Assert.Same(initialObject, reloadedObject);
      }

      [Fact]
      public void ReturnedObjectsImplementIConfigReloadingProxyInterface()
      {
         var configuration = GetConfig();

         var foo = configuration.GetSection("foo").CreateReloadingProxy<IFoo>();

         var proxyFoo = (ConfigReloadingProxy<IFoo>)foo;

         var initialObject = proxyFoo.Object;
         Assert.IsType<Foo>(initialObject);

         IFoo? reloadingFoo = null;
         IFoo? reloadedFoo = null;

         proxyFoo.Reloading += (s, e) => { reloadingFoo = proxyFoo.Object; };
         proxyFoo.Reloaded += (s, e) => { reloadedFoo = proxyFoo.Object; };

         ChangeConfig(configuration, new KeyValuePair<string, string>("foo:value:qux", "xyz"));

         var changedObject = proxyFoo.Object;

         Assert.NotSame(initialObject, changedObject);
         Assert.Same(initialObject, reloadingFoo);
         Assert.Same(changedObject, reloadedFoo);
      }

      [Fact]
      public void SpecifyingReloadOnChangeInConfigCausesCreateExtensionToReturnReloadingProxy()
      {
         var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
         {
            ["foo:type"] = typeof(Foo).AssemblyQualifiedName!,
            ["foo:value:bar"] = "123",
            ["foo:reloadOnChange"] = "true",
         }).Build();

         var foo = config.GetSection("foo").Create<IFoo>();

         Assert.IsAssignableFrom<ConfigReloadingProxy<IFoo>>(foo);
      }

      [Fact]
      public void SpecifyingReloadOnChangeInConfigCausesCreateExtensionToReturnReloadingProxy2()
      {
         var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
         {
            ["foo:value:bar"] = "123",
            ["foo:reloadOnChange"] = "true",
         }).Build();

         var foo = config.GetSection("foo").Create<IFoo>(new DefaultTypes().Add(typeof(IFoo), typeof(Foo)));

         Assert.IsAssignableFrom<ConfigReloadingProxy<IFoo>>(foo);
      }

      private static IConfigurationRoot GetConfig(params KeyValuePair<string, string>[] additionalSettings)
      {
         var initialData = new Dictionary<string, string>
         {
            ["foo:type"] = typeof(Foo).AssemblyQualifiedName!,
            ["foo:value:bar"] = "123",
            ["bar:type"] = typeof(Bar).AssemblyQualifiedName!,
            ["bar:value:qux"] = "5",
            ["baz:type"] = typeof(Baz).AssemblyQualifiedName!,
            ["baz:value:qux"] = "7",
         };
         foreach (var setting in additionalSettings)
            initialData.Add(setting.Key, setting.Value);
         return new ConfigurationBuilder().AddInMemoryCollection(initialData).Build();
      }

      private static void ChangeConfig(IConfigurationRoot root, params KeyValuePair<string, string>[] additionalSettings)
      {
         ChangeConfig(root, new Dictionary<string, string> { { "foo:value:bar", "456" }, { "bar:value:qux", "10" }, { "baz:value:foo", "11" } }.Concat(additionalSettings));
      }

      private static void ChangeConfig(IConfigurationRoot root, IEnumerable<KeyValuePair<string, string>> settings)
      {
         var provider = (MemoryConfigurationProvider)root.Providers.First();
         foreach (var setting in settings)
            provider.Set(setting.Key, setting.Value);
         var _onReloadMethod = typeof(ConfigurationProvider).GetMethod("OnReload", BindingFlags.Instance | BindingFlags.NonPublic)!;
         _onReloadMethod.Invoke(provider, null);
      }

#pragma warning disable CA1034 // Nested types should not be visible
      public interface IFooBase
      {
         int Bar { get; }
      }

      public interface IFoo : IFooBase
      {
         int Baz();
         string? Qux { get; set; }
      }

      public class Foo : IFoo
      {
         public Foo(int bar)
         {
            Bar = bar;
         }

         public int Bar { get; }
         public int Baz() => Bar * 2;
         public string? Qux { get; set; }
      }

      public interface IBar
      {
         int Qux { get; }
         event EventHandler? Baz;
      }

      public class Bar : IBar
      {
         public Bar(int qux)
         {
            Qux = qux;
         }

         public int Qux { get; }

         public event EventHandler? Baz;

         public void OnBaz()
         {
            Baz?.Invoke(this, EventArgs.Empty);
         }
      }

      public interface IBaz
      {
         int Foo { get; set; }
         bool IsDisposed { get; }
      }

#pragma warning disable CA1063 // Implement IDisposable Correctly
      public class Baz : IBaz, IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
      {
         public int Foo { get; set; }
         public bool IsDisposed { get; private set; }

#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
         public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
         {
            IsDisposed = true;
         }
      }

      public interface IGrault
      {
         int Waldo { get; }
         IGarply Garply { get; }
      }

      public class Grault : IGrault
      {
         public Grault(int waldo, IGarply garply)
         {
            Waldo = waldo;
            Garply = garply;
         }

         public int Waldo { get; }
         public IGarply Garply { get; }
      }

#pragma warning disable CA1040 // Avoid empty interfaces
      public interface IGarply { }
#pragma warning restore CA1040 // Avoid empty interfaces

      public class Garply : IGarply { }
#pragma warning restore CA1034 // Nested types should not be visible
   }
}
