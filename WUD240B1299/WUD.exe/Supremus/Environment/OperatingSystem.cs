namespace Supremus.Environment
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class OperatingSystem
    {
        private int wBuild;
        private int wMajor;
        private int wMinor;
        private PlatformID wPlatform;
        private ProcessorArchitectures wProcessorArchitecture;
        private string wProcessorArchitectureName;
        private Products wProduct;
        private ProductBases wProductBase;
        private string wProductBaseName;
        private ProductFamilies wProductFamily;
        private string wProductFamilyName;
        private string wProductName;
        private ProductTypes wProductType;
        private string wProductTypeName;
        private string wServicePackName;

        public OperatingSystem()
        {
            IntPtr ptr;
            this.wProductName = "Unknown";
            this.wProductBaseName = "Unknown";
            this.wProductFamilyName = "Unknown";
            this.wProductTypeName = "Unknown";
            this.wProcessorArchitectureName = "Unknown";
            this.wServicePackName = "";
            OSVERSIONINFO structure = new OSVERSIONINFO();
            structure.dwOSVersionInfoSize = (uint) Marshal.SizeOf(structure);
            SYSTEM_INFO lpSystemInfo = new SYSTEM_INFO();
            GetVersionEx(ref structure);
            this.wPlatform = (PlatformID) structure.dwPlatformId;
            this.wMajor = (int) structure.dwMajorVersion;
            this.wMinor = (int) structure.dwMinorVersion;
            this.wBuild = (int) structure.dwBuildNumber;
            if (((this.wMajor == 5) && (this.wMinor >= 1)) || (this.wMajor > 5))
            {
                GetNativeSystemInfo(ref lpSystemInfo);
            }
            else
            {
                GetSystemInfo(ref lpSystemInfo);
            }
            switch (this.wPlatform)
            {
                case PlatformID.Win32:
                    if (this.wMajor == 4)
                    {
                        switch (this.wMinor)
                        {
                            case 10:
                                switch (this.wBuild)
                                {
                                    case 0x7ce:
                                        this.wProduct = Products.Windows98;
                                        this.wProductName = "Microsoft Windows 98";
                                        break;

                                    case 0x8ae:
                                        this.wProduct = Products.Windows98SE;
                                        this.wProductName = "Microsoft Windows 98 Second Edition";
                                        break;
                                }
                                break;

                            case 90:
                                this.wProduct = Products.WindowsME;
                                this.wProductName = "Microsoft Windows Millennium Edition";
                                break;
                        }
                    }
                    goto Label_0937;

                case PlatformID.WinNT:
                {
                    ptr = LoadLibrary("shlwapi.dll");
                    IsOSInvoker delegateForFunctionPointer = (IsOSInvoker) Marshal.GetDelegateForFunctionPointer(GetProcAddress(ptr, 0x1b5), typeof(IsOSInvoker));
                    switch (this.wMajor)
                    {
                        case 5:
                            switch (this.wMinor)
                            {
                                case 0:
                                    if (!delegateForFunctionPointer(11))
                                    {
                                        if (delegateForFunctionPointer(10))
                                        {
                                            this.wProduct = Products.Windows2000AdvancedServer;
                                            this.wProductName = "Microsoft Windows 2000 Advanced Server";
                                        }
                                        else if (delegateForFunctionPointer(9))
                                        {
                                            this.wProduct = Products.Windows2000Server;
                                            this.wProductName = "Microsoft Windows 2000 Server";
                                        }
                                        else if (delegateForFunctionPointer(8))
                                        {
                                            this.wProduct = Products.Windows2000Professional;
                                            this.wProductName = "Microsoft Windows 2000 Professional";
                                        }
                                        break;
                                    }
                                    this.wProduct = Products.Windows2000DatacenterServer;
                                    this.wProductName = "Microsoft Windows 2000 Datacenter Server";
                                    break;

                                case 1:
                                    if (GetSystemMetrics(0x58) == 0)
                                    {
                                        if (delegateForFunctionPointer(0x23))
                                        {
                                            this.wProduct = Products.WindowsXPMediaCenter;
                                            this.wProductName = "Microsoft Windows XP Media Center";
                                        }
                                        else if (delegateForFunctionPointer(0x21))
                                        {
                                            this.wProduct = Products.WindowsXPTabletPC;
                                            this.wProductName = "Microsoft Windows XP Tablet PC Edition";
                                        }
                                        else if (delegateForFunctionPointer(0x13))
                                        {
                                            this.wProduct = Products.WindowsXPHome;
                                            this.wProductName = "Microsoft Windows XP Home Edition";
                                        }
                                        else if (delegateForFunctionPointer(8))
                                        {
                                            this.wProduct = Products.WindowsXPProfessional;
                                            this.wProductName = "Microsoft Windows XP Professional";
                                        }
                                        break;
                                    }
                                    this.wProduct = Products.WindowsXPStarter;
                                    this.wProductName = "Microsoft Windows XP Starter Edition";
                                    break;

                                case 2:
                                    if ((structure.wSuiteMask & 0x8000) != 0x8000)
                                    {
                                        if ((structure.wSuiteMask & 0x4000) == 0x4000)
                                        {
                                            this.wProduct = Products.WindowsComputeClusterServer2003;
                                            this.wProductName = "Microsoft Windows Compute Cluster Server 2003";
                                        }
                                        else if ((structure.wSuiteMask & 0x2000) == 0x2000)
                                        {
                                            this.wProduct = Products.WindowsStorageServer2003;
                                            this.wProductName = "Microsoft Windows Storage Server 2003";
                                        }
                                        else if (delegateForFunctionPointer(0x20))
                                        {
                                            this.wProduct = Products.WindowsSmallBusinessServer2003;
                                            this.wProductName = "Microsoft Windows Small Business Server 2003";
                                        }
                                        else if (GetSystemMetrics(0x59) != 0)
                                        {
                                            if (delegateForFunctionPointer(0x1f))
                                            {
                                                this.wProduct = Products.WindowsServer2003R2Web;
                                                this.wProductName = "Microsoft Windows Server 2003 R2 Web Edition";
                                            }
                                            else if (delegateForFunctionPointer(0x17))
                                            {
                                                this.wProduct = Products.WindowsServer2003R2Datacenter;
                                                this.wProductName = "Microsoft Windows Server 2003 R2 Datacenter Edition";
                                            }
                                            else if (delegateForFunctionPointer(0x16))
                                            {
                                                this.wProduct = Products.WindowsServer2003R2Enterprise;
                                                this.wProductName = "Microsoft Windows Server 2003 R2 Enterprise Edition";
                                            }
                                            else if (delegateForFunctionPointer(0x15))
                                            {
                                                this.wProduct = Products.WindowsServer2003R2Standard;
                                                this.wProductName = "Microsoft Windows Server 2003 R2 Standard Edition";
                                            }
                                        }
                                        else if (delegateForFunctionPointer(0x1f))
                                        {
                                            this.wProduct = Products.WindowsServer2003Web;
                                            this.wProductName = "Microsoft Windows Server 2003 Web Edition";
                                        }
                                        else if (delegateForFunctionPointer(0x17))
                                        {
                                            this.wProduct = Products.WindowsServer2003Datacenter;
                                            this.wProductName = "Microsoft Windows Server 2003 Datacenter Edition";
                                        }
                                        else if (delegateForFunctionPointer(0x16))
                                        {
                                            this.wProduct = Products.WindowsServer2003Enterprise;
                                            this.wProductName = "Microsoft Windows Server 2003 Enterprise Edition";
                                        }
                                        else if (delegateForFunctionPointer(0x15))
                                        {
                                            this.wProduct = Products.WindowsServer2003Standard;
                                            this.wProductName = "Microsoft Windows Server 2003 Standard Edition";
                                        }
                                        break;
                                    }
                                    this.wProduct = Products.WindowsHomeServer;
                                    this.wProductName = "Microsoft Windows Home Server";
                                    break;
                            }
                            goto Label_0930;

                        case 6:
                            switch (structure.wProductType)
                            {
                                case 1:
                                    this.wProduct = Products.WindowsVistaUltimate;
                                    this.wProductName = "Microsoft Windows Vista Ultimate";
                                    break;

                                case 2:
                                    this.wProduct = Products.WindowsVistaHomeBasic;
                                    this.wProductName = "Microsoft Windows Vista Home Basic";
                                    break;

                                case 3:
                                    this.wProduct = Products.WindowsVistaHomePremium;
                                    this.wProductName = "Microsoft Windows Vista Home Premium";
                                    break;

                                case 4:
                                    this.wProduct = Products.WindowsVistaEnterprise;
                                    this.wProductName = "Microsoft Windows Vista Enterprise";
                                    break;

                                case 5:
                                    this.wProduct = Products.WindowsVistaHomeBasicN;
                                    this.wProductName = "Microsoft Windows Vista Home Basic N";
                                    break;

                                case 6:
                                    this.wProduct = Products.WindowsVistaBusiness;
                                    this.wProductName = "Microsoft Windows Vista Business";
                                    break;

                                case 7:
                                    this.wProduct = Products.WindowsServer2008Standard;
                                    this.wProductName = "Microsoft Windows Server 2008 Standard";
                                    break;

                                case 8:
                                    this.wProduct = Products.WindowsServer2008Datacenter;
                                    this.wProductName = "Microsoft Windows Server 2008 Datacenter";
                                    break;

                                case 9:
                                    this.wProduct = Products.WindowsSmallBusinessServer2008;
                                    this.wProductName = "Microsoft Windows Small Business Server 2008";
                                    break;

                                case 10:
                                    this.wProduct = Products.WindowsServer2008Enterprise;
                                    this.wProductName = "Microsoft Windows Server 2008 Enterprise";
                                    break;

                                case 11:
                                    this.wProduct = Products.WindowsVistaStarter;
                                    this.wProductName = "Microsoft Windows Vista Starter";
                                    break;

                                case 12:
                                    this.wProduct = Products.WindowsServer2008DatacenterCore;
                                    this.wProductName = "Microsoft Windows Server 2008 Datacenter Core";
                                    break;

                                case 13:
                                    this.wProduct = Products.WindowsServer2008StandardCore;
                                    this.wProductName = "Microsoft Windows Server 2008 Standard Core";
                                    break;

                                case 14:
                                    this.wProduct = Products.WindowsServer2008EnterpriseCore;
                                    this.wProductName = "Microsoft Windows Server 2008 Enterprise Core";
                                    break;

                                case 15:
                                    this.wProduct = Products.WindowsServer2008Enterprise;
                                    this.wProductName = "Microsoft Windows Server 2008 Enterprise";
                                    break;

                                case 0x10:
                                    this.wProduct = Products.WindowsVistaBusinessN;
                                    this.wProductName = "Microsoft Windows Vista Business N";
                                    break;

                                case 0x11:
                                    this.wProduct = Products.WindowsServer2008Web;
                                    this.wProductName = "Microsoft Windows Web Server 2008";
                                    break;

                                case 0x12:
                                    this.wProduct = Products.WindowsHPCServer2008;
                                    this.wProductName = "Microsoft Windows HPC Server 2008";
                                    break;

                                case 20:
                                    this.wProduct = Products.WindowsStorageServer2008Express;
                                    this.wProductName = "Microsoft Windows Storage Server 2008 Express";
                                    break;

                                case 0x15:
                                    this.wProduct = Products.WindowsStorageServer2008Standard;
                                    this.wProductName = "Microsoft Windows Storage Server 2008 Standard";
                                    break;

                                case 0x16:
                                    this.wProduct = Products.WindowsStorageServer2008Workgroup;
                                    this.wProductName = "Microsoft Windows Storage Server 2008 Workgroup";
                                    break;

                                case 0x17:
                                    this.wProduct = Products.WindowsStorageServer2008Enterprise;
                                    this.wProductName = "Microsoft Windows Storage Server 2008 Enterprise";
                                    break;

                                case 0x18:
                                    this.wProduct = Products.WindowsEssentialBusinessServer2008;
                                    this.wProductName = "Microsoft Windows Essential Business Server 2008";
                                    break;

                                case 0x1a:
                                    this.wProduct = Products.WindowsVistaHomePremiumN;
                                    this.wProductName = "Microsoft Windows Vista Home Premium N";
                                    break;

                                case 0x1b:
                                    this.wProduct = Products.WindowsVistaEnterpriseN;
                                    this.wProductName = "Microsoft Windows Vista Enterprise N";
                                    break;

                                case 0x1c:
                                    this.wProduct = Products.WindowsVistaUltimateN;
                                    this.wProductName = "Microsoft Windows Vista Ultimate N";
                                    break;

                                case 0x1d:
                                    this.wProduct = Products.WindowsServer2008WebCore;
                                    this.wProductName = "Microsoft Windows Web Server 2008 Core";
                                    this.wProductTypeName = "Server";
                                    break;

                                case 30:
                                    this.wProduct = Products.WindowsEssentialBusinessServer2008;
                                    this.wProductName = "Microsoft Windows Essential Business Server 2008";
                                    break;

                                case 0x1f:
                                    this.wProduct = Products.WindowsEssentialBusinessServer2008;
                                    this.wProductName = "Microsoft Windows Essential Business Server 2008";
                                    break;

                                case 0x20:
                                    this.wProduct = Products.WindowsEssentialBusinessServer2008;
                                    this.wProductName = "Microsoft Windows Essential Business Server 2008";
                                    break;

                                case 0x23:
                                    this.wProduct = Products.WindowsEssentialBusinessServer2008WithoutHyperV;
                                    this.wProductName = "Microsoft Windows Essential Business Server 2008 Without Hyper-V";
                                    break;

                                case 0x24:
                                    this.wProduct = Products.WindowsServer2008StandardWithoutHyperV;
                                    this.wProductName = "Microsoft Windows Server 2008 Standard Without Hyper-V";
                                    break;

                                case 0x25:
                                    this.wProduct = Products.WindowsServer2008DatacenterWithoutHyperV;
                                    this.wProductName = "Microsoft Windows Server 2008 Datacenter Without Hyper-V";
                                    break;

                                case 0x26:
                                    this.wProduct = Products.WindowsServer2008EnterpriseWithoutHyperV;
                                    this.wProductName = "Microsoft Windows Server 2008 Enterprise Without Hyper-V";
                                    break;

                                case 0x27:
                                    this.wProduct = Products.WindowsServer2008DatacenterCoreWithoutHyperV;
                                    this.wProductName = "Microsoft Windows Server 2008 Datacenter Core Without Hyper-V";
                                    break;

                                case 40:
                                    this.wProduct = Products.WindowsServer2008StandardCoreWithoutHyperV;
                                    this.wProductName = "Microsoft Windows Server 2008 Standard Core Without Hyper-V";
                                    break;

                                case 0x29:
                                    this.wProduct = Products.WindowsServer2008EnterpriseCoreWithoutHyperV;
                                    this.wProductName = "Microsoft Windows Server 2008 Enterprise Core Without Hyper-V";
                                    break;

                                case 0x2a:
                                    this.wProduct = Products.HyperVServer2008;
                                    this.wProductName = "Microsoft Hyper-V Server 2008";
                                    break;
                            }
                            goto Label_0930;
                    }
                    break;
                }
                default:
                    goto Label_0937;
            }
        Label_0930:
            FreeLibrary(ptr);
        Label_0937:
            switch (this.wProduct)
            {
                case Products.Windows98:
                    this.wProductBase = ProductBases.Windows98;
                    this.wProductBaseName = "Microsoft Windows 98";
                    break;

                case Products.Windows98SE:
                    this.wProductBase = ProductBases.Windows98SE;
                    this.wProductBaseName = "Microsoft Windows 98 Second Edition";
                    break;

                case Products.WindowsME:
                    this.wProductBase = ProductBases.WindowsME;
                    this.wProductBaseName = "Microsoft Windows Millennium Edition";
                    break;

                case Products.Windows2000Professional:
                    this.wProductBase = ProductBases.Windows2000Professional;
                    this.wProductBaseName = "Microsoft Windows 2000 Professional";
                    break;

                case Products.Windows2000Server:
                    this.wProductBase = ProductBases.Windows2000Server;
                    this.wProductBaseName = "Microsoft Windows 2000 Server";
                    break;

                case Products.Windows2000AdvancedServer:
                    this.wProductBase = ProductBases.Windows2000AdvancedServer;
                    this.wProductBaseName = "Microsoft Windows 2000 Advanced Server";
                    break;

                case Products.Windows2000DatacenterServer:
                    this.wProductBase = ProductBases.Windows2000DatacenterServer;
                    this.wProductBaseName = "Microsoft Windows 2000 Datacenter Server";
                    break;

                case Products.WindowsXPStarter:
                    this.wProductBase = ProductBases.WindowsXPStarter;
                    this.wProductBaseName = "Microsoft Windows XP Starter Edition";
                    break;

                case Products.WindowsXPHome:
                    this.wProductBase = ProductBases.WindowsXPHome;
                    this.wProductBaseName = "Microsoft Windows XP Home Edition";
                    break;

                case Products.WindowsXPProfessional:
                    this.wProductBase = ProductBases.WindowsXPProfessional;
                    this.wProductBaseName = "Microsoft Windows XP Professional";
                    break;

                case Products.WindowsXPTabletPC:
                    this.wProductBase = ProductBases.WindowsXPTabletPC;
                    this.wProductBaseName = "Microsoft Windows XP Tablet PC Edition";
                    break;

                case Products.WindowsXPMediaCenter:
                    this.wProductBase = ProductBases.WindowsXPMediaCenter;
                    this.wProductBaseName = "Microsoft Windows XP Media Center";
                    break;

                case Products.WindowsServer2003Web:
                case Products.WindowsServer2003R2Web:
                    this.wProductBase = ProductBases.WindowsServer2003Web;
                    this.wProductBaseName = "Microsoft Windows Server 2003 Web Edition";
                    break;

                case Products.WindowsServer2003Standard:
                case Products.WindowsServer2003R2Standard:
                    this.wProductBase = ProductBases.WindowsServer2003Standard;
                    this.wProductBaseName = "Microsoft Windows Server 2003 Standard Edition";
                    break;

                case Products.WindowsServer2003Enterprise:
                case Products.WindowsServer2003R2Enterprise:
                    this.wProductBase = ProductBases.WindowsServer2003Enterprise;
                    this.wProductBaseName = "Microsoft Windows Server 2003 Enterprise Edition";
                    break;

                case Products.WindowsServer2003Datacenter:
                case Products.WindowsServer2003R2Datacenter:
                    this.wProductBase = ProductBases.WindowsServer2003Datacenter;
                    this.wProductBaseName = "Microsoft Windows Server 2003 Datacenter Edition";
                    break;

                case Products.WindowsComputeClusterServer2003:
                    this.wProductBase = ProductBases.WindowsComputeClusterServer2003;
                    this.wProductBaseName = "Microsoft Windows Compute Cluster Server 2003";
                    break;

                case Products.WindowsSmallBusinessServer2003:
                    this.wProductBase = ProductBases.WindowsSmallBusinessServer2003;
                    this.wProductBaseName = "Microsoft Windows Small Business Server 2003";
                    break;

                case Products.WindowsStorageServer2003:
                    this.wProductBase = ProductBases.WindowsStorageServer2003;
                    this.wProductBaseName = "Microsoft Windows Storage Server 2003";
                    break;

                case Products.WindowsHomeServer:
                    this.wProductBase = ProductBases.WindowsHomeServer;
                    this.wProductBaseName = "Microsoft Windows Home Server";
                    break;

                case Products.WindowsVistaStarter:
                    this.wProductBase = ProductBases.WindowsVistaStarter;
                    this.wProductBaseName = "Microsoft Windows Vista Starter";
                    break;

                case Products.WindowsVistaHomeBasic:
                case Products.WindowsVistaHomeBasicN:
                    this.wProductBase = ProductBases.WindowsVistaHomeBasic;
                    this.wProductBaseName = "Microsoft Windows Vista Home Basic";
                    break;

                case Products.WindowsVistaHomePremium:
                case Products.WindowsVistaHomePremiumN:
                    this.wProductBase = ProductBases.WindowsVistaHomePremium;
                    this.wProductBaseName = "Microsoft Windows Vista Home Premium";
                    break;

                case Products.WindowsVistaBusiness:
                case Products.WindowsVistaBusinessN:
                    this.wProductBase = ProductBases.WindowsVistaBusiness;
                    this.wProductBaseName = "Microsoft Windows Vista Business";
                    break;

                case Products.WindowsVistaEnterprise:
                case Products.WindowsVistaEnterpriseN:
                    this.wProductBase = ProductBases.WindowsVistaEnterprise;
                    this.wProductBaseName = "Microsoft Windows Vista Enterprise";
                    break;

                case Products.WindowsVistaUltimate:
                case Products.WindowsVistaUltimateN:
                    this.wProductBase = ProductBases.WindowsVistaUltimate;
                    this.wProductBaseName = "Microsoft Windows Vista Ultimate";
                    break;

                case Products.WindowsServer2008Web:
                case Products.WindowsServer2008WebCore:
                    this.wProductBase = ProductBases.WindowsServer2008Web;
                    this.wProductBaseName = "Microsoft Windows Web Server 2008";
                    break;

                case Products.WindowsServer2008Standard:
                case Products.WindowsServer2008StandardCore:
                case Products.WindowsServer2008StandardWithoutHyperV:
                case Products.WindowsServer2008StandardCoreWithoutHyperV:
                    this.wProductBase = ProductBases.WindowsServer2008Standard;
                    this.wProductBaseName = "Microsoft Windows Server 2008 Standard";
                    break;

                case Products.WindowsServer2008Enterprise:
                case Products.WindowsServer2008EnterpriseCore:
                case Products.WindowsServer2008EnterpriseWithoutHyperV:
                case Products.WindowsServer2008EnterpriseCoreWithoutHyperV:
                    this.wProductBase = ProductBases.WindowsServer2008Enterprise;
                    this.wProductBaseName = "Microsoft Windows Server 2008 Enterprise";
                    break;

                case Products.WindowsServer2008Datacenter:
                case Products.WindowsServer2008DatacenterCore:
                case Products.WindowsServer2008DatacenterWithoutHyperV:
                case Products.WindowsServer2008DatacenterCoreWithoutHyperV:
                    this.wProductBase = ProductBases.WindowsServer2008Datacenter;
                    this.wProductBaseName = "Microsoft Windows Server 2008 Datacenter";
                    break;

                case Products.WindowsHPCServer2008:
                    this.wProductBase = ProductBases.WindowsServer2008Web;
                    this.wProductBaseName = "Microsoft Windows HPC Server 2008";
                    break;

                case Products.WindowsSmallBusinessServer2008:
                    this.wProductBase = ProductBases.WindowsSmallBusinessServer2008;
                    this.wProductBaseName = "Microsoft Windows Small Business Server 2008";
                    break;

                case Products.WindowsEssentialBusinessServer2008:
                    this.wProductBase = ProductBases.WindowsEssentialBusinessServer2008;
                    this.wProductBaseName = "Microsoft Windows Essential Business Server 2008";
                    break;

                case Products.WindowsStorageServer2008Express:
                    this.wProductBase = ProductBases.WindowsStorageServer2008Express;
                    this.wProductBaseName = "Microsoft Windows Storage Server 2008 Express";
                    break;

                case Products.WindowsStorageServer2008Workgroup:
                    this.wProductBase = ProductBases.WindowsStorageServer2008Workgroup;
                    this.wProductBaseName = "Microsoft Windows Storage Server 2008 Workgroup";
                    break;

                case Products.WindowsStorageServer2008Standard:
                    this.wProductBase = ProductBases.WindowsStorageServer2008Standard;
                    this.wProductBaseName = "Microsoft Windows Storage Server 2008 Standard";
                    break;

                case Products.WindowsStorageServer2008Enterprise:
                    this.wProductBase = ProductBases.WindowsStorageServer2008Enterprise;
                    this.wProductBaseName = "Microsoft Windows Storage Server 2008 Enterprise";
                    break;

                case Products.HyperVServer2008:
                    this.wProductBase = ProductBases.HyperVServer2008;
                    this.wProductBaseName = "Microsoft Hyper-V Server 2008";
                    break;
            }
            switch (this.wProductBase)
            {
                case ProductBases.Windows98:
                case ProductBases.Windows98SE:
                    this.wProductFamily = ProductFamilies.Windows98;
                    this.wProductFamilyName = "Microsoft Windows 98";
                    break;

                case ProductBases.WindowsME:
                    this.wProductFamily = ProductFamilies.WindowsME;
                    this.wProductFamilyName = "Microsoft Windows Millennium Edition";
                    break;

                case ProductBases.Windows2000Professional:
                case ProductBases.Windows2000Server:
                case ProductBases.Windows2000AdvancedServer:
                case ProductBases.Windows2000DatacenterServer:
                    this.wProductFamily = ProductFamilies.Windows2000;
                    this.wProductFamilyName = "Microsoft Windows 2000";
                    break;

                case ProductBases.WindowsXPStarter:
                case ProductBases.WindowsXPHome:
                case ProductBases.WindowsXPProfessional:
                case ProductBases.WindowsXPTabletPC:
                case ProductBases.WindowsXPMediaCenter:
                    this.wProductFamily = ProductFamilies.WindowsXP;
                    this.wProductFamilyName = "Microsoft Windows XP";
                    break;

                case ProductBases.WindowsServer2003Web:
                case ProductBases.WindowsServer2003Standard:
                case ProductBases.WindowsServer2003Enterprise:
                case ProductBases.WindowsServer2003Datacenter:
                    this.wProductFamily = ProductFamilies.WindowsServer2003;
                    this.wProductFamilyName = "Microsoft Windows Server 2003";
                    break;

                case ProductBases.WindowsComputeClusterServer2003:
                    this.wProductFamily = ProductFamilies.WindowsComputeClusterServer2003;
                    this.wProductFamilyName = "Microsoft Windows Compute Cluster Server 2003";
                    break;

                case ProductBases.WindowsSmallBusinessServer2003:
                    this.wProductFamily = ProductFamilies.WindowsSmallBusinessServer2003;
                    this.wProductFamilyName = "Microsoft Windows Small Business Server 2003";
                    break;

                case ProductBases.WindowsStorageServer2003:
                    this.wProductFamily = ProductFamilies.WindowsStorageServer2003;
                    this.wProductFamilyName = "Microsoft Windows Storage Server 2003";
                    break;

                case ProductBases.WindowsHomeServer:
                    this.wProductFamily = ProductFamilies.WindowsHomeServer;
                    this.wProductFamilyName = "Microsoft Windows Home Server";
                    break;

                case ProductBases.WindowsVistaStarter:
                case ProductBases.WindowsVistaHomeBasic:
                case ProductBases.WindowsVistaHomePremium:
                case ProductBases.WindowsVistaBusiness:
                case ProductBases.WindowsVistaEnterprise:
                case ProductBases.WindowsVistaUltimate:
                    this.wProductFamily = ProductFamilies.WindowsVista;
                    this.wProductFamilyName = "Microsoft Windows Vista";
                    break;

                case ProductBases.WindowsServer2008Web:
                case ProductBases.WindowsServer2008Standard:
                case ProductBases.WindowsServer2008Enterprise:
                case ProductBases.WindowsServer2008Datacenter:
                    this.wProductFamily = ProductFamilies.WindowsServer2008;
                    this.wProductFamilyName = "Microsoft Windows Server 2008";
                    break;

                case ProductBases.WindowsHPCServer2008:
                    this.wProductFamily = ProductFamilies.WindowsServer2008;
                    this.wProductFamilyName = "Microsoft Windows Server 2008";
                    break;

                case ProductBases.WindowsSmallBusinessServer2008:
                    this.wProductFamily = ProductFamilies.WindowsSmallBusinessServer2008;
                    this.wProductFamilyName = "Microsoft Windows Small Business Server 2008";
                    break;

                case ProductBases.WindowsEssentialBusinessServer2008:
                    this.wProductFamily = ProductFamilies.WindowsEssentialBusinessServer2008;
                    this.wProductFamilyName = "Microsoft Windows Essential Business Server 2008";
                    break;

                case ProductBases.WindowsStorageServer2008Express:
                case ProductBases.WindowsStorageServer2008Workgroup:
                case ProductBases.WindowsStorageServer2008Standard:
                case ProductBases.WindowsStorageServer2008Enterprise:
                    this.wProductFamily = ProductFamilies.WindowsStorageServer2008;
                    this.wProductFamilyName = "Microsoft Windows Storage Server 2008";
                    break;

                case ProductBases.HyperVServer2008:
                    this.wProductFamily = ProductFamilies.HyperVServer2008;
                    this.wProductFamilyName = "Microsoft Hyper-V Server 2008";
                    break;
            }
            switch (this.wProductFamily)
            {
                case ProductFamilies.Windows98:
                case ProductFamilies.WindowsME:
                case ProductFamilies.WindowsXP:
                case ProductFamilies.WindowsVista:
                    this.wProductType = ProductTypes.Workstation;
                    this.wProductTypeName = "Workstation";
                    break;

                case ProductFamilies.Windows2000:
                case ProductFamilies.WindowsComputeClusterServer2003:
                case ProductFamilies.WindowsSmallBusinessServer2003:
                case ProductFamilies.WindowsStorageServer2003:
                case ProductFamilies.WindowsHomeServer:
                case ProductFamilies.WindowsServer2008:
                case ProductFamilies.WindowsHPCServer2008:
                case ProductFamilies.WindowsSmallBusinessServer2008:
                case ProductFamilies.WindowsEssentialBusinessServer2008:
                case ProductFamilies.WindowsStorageServer2008:
                    this.wProductType = ProductTypes.Server;
                    this.wProductTypeName = "Server";
                    break;
            }
            switch (lpSystemInfo.wProcessorArchitecture)
            {
                case 0:
                    this.wProcessorArchitecture = ProcessorArchitectures.x86;
                    break;

                case 6:
                    this.wProcessorArchitecture = ProcessorArchitectures.ia64;
                    break;

                case 9:
                    this.wProcessorArchitecture = ProcessorArchitectures.x64;
                    break;
            }
            switch (this.wProcessorArchitecture)
            {
                case ProcessorArchitectures.x86:
                    this.wProcessorArchitectureName = "x86";
                    break;

                case ProcessorArchitectures.x64:
                    this.wProcessorArchitectureName = "x64";
                    break;
            }
            if (structure.szCSDVersion.Length > 0)
            {
                this.wServicePackName = structure.szCSDVersion;
            }
        }

        [DllImport("kernel32.dll")]
        internal static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll")]
        internal static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);
        [DllImport("kernel32.dll", CharSet=CharSet.Ansi)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, uint procName);
        [DllImport("kernel32.dll")]
        internal static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);
        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(uint smIndex);
        [DllImport("kernel32.dll")]
        internal static extern bool GetVersionEx(ref OSVERSIONINFO osvi);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        public ProcessorArchitectures ProcessorArchitecture =>
            this.wProcessorArchitecture;

        public string ProcessorArchitectureName =>
            this.wProcessorArchitectureName;

        public Products Product =>
            this.wProduct;

        public ProductBases ProductBase =>
            this.wProductBase;

        public string ProductBaseName =>
            this.wProductBaseName;

        public ProductFamilies ProductFamily =>
            this.wProductFamily;

        public string ProductFamilyName =>
            this.wProductFamilyName;

        public string ProductName =>
            this.wProductName;

        public ProductTypes ProductType =>
            this.wProductType;

        public string ProductTypeName =>
            this.wProductTypeName;

        public string ServicePackName =>
            this.wServicePackName;

        internal delegate bool IsOSInvoker(uint dwOS);

        [StructLayout(LayoutKind.Sequential)]
        internal struct OSVERSIONINFO
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        private enum PlatformID
        {
            Win32 = 1,
            WinNT = 2
        }

        public enum ProcessorArchitectures
        {
            Unknown,
            x86,
            x64,
            ia64
        }

        public enum ProductBases
        {
            Unknown,
            Windows98,
            Windows98SE,
            WindowsME,
            Windows2000Professional,
            Windows2000Server,
            Windows2000AdvancedServer,
            Windows2000DatacenterServer,
            WindowsXPStarter,
            WindowsXPHome,
            WindowsXPProfessional,
            WindowsXPTabletPC,
            WindowsXPMediaCenter,
            WindowsServer2003Web,
            WindowsServer2003Standard,
            WindowsServer2003Enterprise,
            WindowsServer2003Datacenter,
            WindowsComputeClusterServer2003,
            WindowsSmallBusinessServer2003,
            WindowsStorageServer2003,
            WindowsHomeServer,
            WindowsVistaStarter,
            WindowsVistaHomeBasic,
            WindowsVistaHomePremium,
            WindowsVistaBusiness,
            WindowsVistaEnterprise,
            WindowsVistaUltimate,
            WindowsServer2008Web,
            WindowsServer2008Standard,
            WindowsServer2008Enterprise,
            WindowsServer2008Datacenter,
            WindowsHPCServer2008,
            WindowsSmallBusinessServer2008,
            WindowsEssentialBusinessServer2008,
            WindowsStorageServer2008Express,
            WindowsStorageServer2008Workgroup,
            WindowsStorageServer2008Standard,
            WindowsStorageServer2008Enterprise,
            HyperVServer2008
        }

        public enum ProductFamilies
        {
            Unknown,
            Windows98,
            WindowsME,
            Windows2000,
            WindowsXP,
            WindowsServer2003,
            WindowsComputeClusterServer2003,
            WindowsSmallBusinessServer2003,
            WindowsStorageServer2003,
            WindowsHomeServer,
            WindowsVista,
            WindowsServer2008,
            WindowsHPCServer2008,
            WindowsSmallBusinessServer2008,
            WindowsEssentialBusinessServer2008,
            WindowsStorageServer2008,
            HyperVServer2008
        }

        public enum Products
        {
            Unknown,
            Windows98,
            Windows98SE,
            WindowsME,
            Windows2000Professional,
            Windows2000Server,
            Windows2000AdvancedServer,
            Windows2000DatacenterServer,
            WindowsXPStarter,
            WindowsXPHome,
            WindowsXPProfessional,
            WindowsXPTabletPC,
            WindowsXPMediaCenter,
            WindowsServer2003Web,
            WindowsServer2003Standard,
            WindowsServer2003Enterprise,
            WindowsServer2003Datacenter,
            WindowsServer2003R2Web,
            WindowsServer2003R2Standard,
            WindowsServer2003R2Enterprise,
            WindowsServer2003R2Datacenter,
            WindowsComputeClusterServer2003,
            WindowsSmallBusinessServer2003,
            WindowsStorageServer2003,
            WindowsHomeServer,
            WindowsVistaStarter,
            WindowsVistaHomeBasic,
            WindowsVistaHomeBasicN,
            WindowsVistaHomePremium,
            WindowsVistaHomePremiumN,
            WindowsVistaBusiness,
            WindowsVistaBusinessN,
            WindowsVistaEnterprise,
            WindowsVistaEnterpriseN,
            WindowsVistaUltimate,
            WindowsVistaUltimateN,
            WindowsServer2008Web,
            WindowsServer2008WebCore,
            WindowsServer2008Standard,
            WindowsServer2008StandardCore,
            WindowsServer2008StandardWithoutHyperV,
            WindowsServer2008StandardCoreWithoutHyperV,
            WindowsServer2008Enterprise,
            WindowsServer2008EnterpriseCore,
            WindowsServer2008EnterpriseWithoutHyperV,
            WindowsServer2008EnterpriseCoreWithoutHyperV,
            WindowsServer2008Datacenter,
            WindowsServer2008DatacenterCore,
            WindowsServer2008DatacenterWithoutHyperV,
            WindowsServer2008DatacenterCoreWithoutHyperV,
            WindowsHPCServer2008,
            WindowsSmallBusinessServer2008,
            WindowsEssentialBusinessServer2008,
            WindowsEssentialBusinessServer2008WithoutHyperV,
            WindowsStorageServer2008Express,
            WindowsStorageServer2008Workgroup,
            WindowsStorageServer2008Standard,
            WindowsStorageServer2008Enterprise,
            HyperVServer2008
        }

        public enum ProductTypes
        {
            Unknown,
            Workstation,
            Server
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            public ushort wProcessorArchitecture;
            private ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        }
    }
}

