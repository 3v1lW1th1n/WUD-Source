<em>If you wish to, donate to the original creator of this content (<strong>not me</strong>) via paypal: <code>admin&#x0040;windowsupdatesdownloader&#x002E;com</code>.</em>

<hr/>

<h1>WUD-Source</h1>

<table>
<thead>
<tr><td>name</td><td>C# source-folder</td><td>binary-folder</td></tr>
</thead>
<tbody>
<tr><td>Windows Updates Downloader - Version 2.50 Build 1002</td><td><a href="WUD250B1002/">WUD250B1002/</a></td><td><a href="__BINARY__/WUD250B1002Setup/">__BINARY__/WUD250B1002Setup/</a></td></tr>
<tr><td>Windows Updates Downloader - Version 2.40 Build 1299</td><td><a href="WUD240B1299/">WUD240B1299/</a></td><td><a href="__BINARY__/WUD240B1299Setup/">__BINARY__/WUD250B1002Setup/</a></td></tr>
<tr><td>WMP11 Integrator - Version 1.1 Build 60</td><td><a href="WINT11B60/">WINT11B60/</a></td><td><a href="__BINARY__/WINT11B60Setup/">__BINARY__/WINT11B60Setup/</a></td></tr>
</thead>
</table>

<hr/>

The <code>__BINARY__/</code>-folder has the original exe-files (without the Inno-installer), 
it also has (some) of the update-lists.

About the <code>ULZ</code> and <code>UL</code> file format:
A <code>UL</code>-file is a XML-file.
<code>ULZ</code> is a ZIP-archive.

<hr/>

the exe-files were reversed to source in-order to resolve the "not finding update-list-file" bug, 
and for education purposes, mainly finding a more useful way to parse the XML based on the current way the program(s) does it, 
in-order to make the downloader more efficient, possibly integrating Aria2C or other external download-manager for faster, parallel, downloading. 


<br/>

&nbsp; 

<br/>

&nbsp; 

<hr/>

<blockquote>
<em>note taken from: http://www.windowsupdatesdownloader.com/UpdateLists.aspx</em><br/>
The Update Lists (ULs) contain the necessary list of updates for each of the versions of Windows listed below. You will also find ULs for other software such as Office and Exchange.
In order to use the ULs, they must be in the same folder as the Windows Updates Downloader program folder. WUD integrates itself in the shell in order to simplify installation of UL files. Simply download and install WUD first, run it once, then come back here to download the ULs which you want to use.
When presented with the option to open or save, simply open the UL and it will automatically install itself in the WUD program folder.
</blockquote>

<em><strong>it didn't worked for me, possible since I've extracted the exe-file from the inno-setup...</strong></em>

<hr/>

<h2>Update-Lists Mirror, with extracted UL-files (those are readable XML-files).</h2>

<hr/>

<h2>English</h2>

<table>
  <thead>
  <tr>
    <th>product</th>
    <th>servicepack</th>
    <th>platform</th>
    <th>updated</th>
    <th>downloads</th>
    <th>file</a></th>
    <th>file (extracted, readable - it's an XML)</a></th>
  </tr>
  </thead>
  <tbody>
  <tr>
    <td>Exchange 2003</td>
    <td>&nbsp;</td>
    <td>x86</td>
    <td>2006-08-19</td>
    <td>52569</td>
    <td><a href="__BINARY__/exchange-2003-x86-enu.ulz">exchange-2003-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/exchange-2003-x86-enu.ul">exchange-2003-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Office 2003</td>
    <td>&nbsp;</td>
    <td>x86</td>
    <td>2006-08-19</td>
    <td>134990</td>
    <td><a href="__BINARY__/office-2003-x86-enu.ulz">office-2003-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/office-2003-x86-enu.ul">office-2003-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 2000 Professional</td>
    <td>Service Pack 4</td>
    <td>x86</td>
    <td>2010-03-11</td>
    <td>29045</td>
    <td><a href="__BINARY__/windows-2000-professional-sp4-x86-enu.ulz">windows-2000-professional-sp4-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-2000-professional-sp4-x86-enu.ul">windows-2000-professional-sp4-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 2003 Server</td>
    <td>Service Pack 2</td>
    <td>x86</td>
    <td>2010-03-11</td>
    <td>43969</td>
    <td><a href="__BINARY__/windows-2003-server-sp2-x86-enu.ulz">windows-2003-server-sp2-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-2003-server-sp2-x86-enu.ul">windows-2003-server-sp2-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 7</td>
    <td>Service Pack 1</td>
    <td>x64</td>
    <td>2015-03-25</td>
    <td>345253</td>
    <td><a href="__BINARY__/windows-7-sp1-x64-enu.ulz">windows-7-sp1-x64-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-7-sp1-x64-enu.ul">windows-7-sp1-x64-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 7</td>
    <td>Service Pack 1</td>
    <td>x86</td>
    <td>2015-03-25</td>
    <td>159017</td>
    <td><a href="__BINARY__/windows-7-sp1-x86-enu.ulz">windows-7-sp1-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-7-sp1-x86-enu.ul">windows-7-sp1-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 8</td>
    <td>&nbsp;</td>
    <td>x64</td>
    <td>2013-12-10</td>
    <td>35672</td>
    <td><a href="__BINARY__/windows-8-rtm-x64-enu.ulz">windows-8-rtm-x64-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-8-rtm-x64-enu.ul">windows-8-rtm-x64-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 8</td>
    <td>&nbsp;</td>
    <td>x86</td>
    <td>2013-12-10</td>
    <td>21273</td>
    <td><a href="__BINARY__/windows-8-rtm-x86-enu.ulz">windows-8-rtm-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-8-rtm-x86-enu.ul">windows-8-rtm-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 8.1</td>
    <td>&nbsp;</td>
    <td>x64</td>
    <td>2014-09-09</td>
    <td>41839</td>
    <td><a href="__BINARY__/windows-8.1-rtm-x64-enu.ulz">windows-8.1-rtm-x64-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-8.1-rtm-x64-enu.ul">windows-8.1-rtm-x64-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 8.1</td>
    <td>&nbsp;</td>
    <td>x86</td>
    <td>2014-09-09</td>
    <td>19629</td>
    <td><a href="__BINARY__/windows-8.1-rtm-x86-enu.ulz">windows-8.1-rtm-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-8.1-rtm-x86-enu.ul">windows-8.1-rtm-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 8.1 Update</td>
    <td>&nbsp;</td>
    <td>x64</td>
    <td>2015-03-25</td>
    <td>47978</td>
    <td><a href="__BINARY__/windows-8.1-update-x64-enu.ulz">windows-8.1-update-x64-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-8.1-update-x64-enu.ul">windows-8.1-update-x64-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows 8.1 Update</td>
    <td>&nbsp;</td>
    <td>x86</td>
    <td>2015-03-25</td>
    <td>22758</td>
    <td><a href="__BINARY__/windows-8.1-update-x86-enu.ulz">windows-8.1-update-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-8.1-update-x86-enu.ul">windows-8.1-update-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows Vista</td>
    <td>Service Pack 1</td>
    <td>x64</td>
    <td>2009-09-12</td>
    <td>25622</td>
    <td><a href="__BINARY__/windows-vista-sp1-x64-enu.ulz">windows-vista-sp1-x64-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-vista-sp1-x64-enu.ul">windows-vista-sp1-x64-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows Vista</td>
    <td>Service Pack 1</td>
    <td>x86</td>
    <td>2009-09-12</td>
    <td>30770</td>
    <td><a href="__BINARY__/windows-vista-sp1-x86-enu.ulz">windows-vista-sp1-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-vista-sp1-x86-enu.ul">windows-vista-sp1-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows Vista</td>
    <td>Service Pack 2</td>
    <td>x64</td>
    <td>2009-09-12</td>
    <td>34487</td>
    <td><a href=windows-vista-sp2-x64-enu.ulz">windows-vista-sp2-x64-enu.ulz</a></td>
    <td><a href=windows-vista-sp2-x64-enu.ul">windows-vista-sp2-x64-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows Vista</td>
    <td>Service Pack 2</td>
    <td>x86</td>
    <td>2009-09-12</td>
    <td>56580</td>
    <td><a href="__BINARY__/windows-vista-sp2-x86-enu.ulz">windows-vista-sp2-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-vista-sp2-x86-enu.ul">windows-vista-sp2-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows XP</td>
    <td>Service Pack 2</td>
    <td>x64</td>
    <td>2015-03-20</td>
    <td>14141</td>
    <td><a href="__BINARY__/windows-xp-sp2-x64-enu.ulz">windows-xp-sp2-x64-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-xp-sp2-x64-enu.ul">windows-xp-sp2-x64-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows XP</td>
    <td>Service Pack 2</td>
    <td>x86</td>
    <td>2008-04-08</td>
    <td>191509</td>
    <td><a href="__BINARY__/windows-xp-sp2-x86-enu.ulz">windows-xp-sp2-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-xp-sp2-x86-enu.ul">windows-xp-sp2-x86-enu.ul</a></td>
  </tr>
  <tr>
    <td>Windows XP</td>
    <td>Service Pack 3</td>
    <td>x86</td>
    <td>2014-05-01</td>
    <td>62739</td>
    <td><a href="__BINARY__/windows-xp-sp3-x86-enu.ulz">windows-xp-sp3-x86-enu.ulz</a></td>
    <td><a href="__BINARY__/windows-xp-sp3-x86-enu.ul">windows-xp-sp3-x86-enu.ul</a></td>
  </tr>
</tbody>
</table>



