
# AgOpenGPS  ****  Guidance software

Most [Stable AgOpenGPS Release](https://github.com/farmerbriantee/AgOpenGPS/releases)

Discussed in detail on the [AgOpengGPS Forum](https://discourse.agopengps.com/)

The [PCB and Firmware Repository](https://github.com/farmerbriantee/AgOpenGPS_Boards)

The [AgOpenGPS Wiki](https://github.com/farmerbriantee/AgOpenGPS_Boards/wiki)


Ag Precision Mapping and Section Control Software

AgOpenGPS is 2 programs. AgIO is the communication hub to the outside world and AgOpenGPS is the 
application. You can run either and within each, you can run the other. 

You only need to run AgOpenGPS if you are using the simulator.

To install click on the "Most Stable Release" link above and download the AgOpenGPS_v5.zip.
Unzip or extract the contents to a folder (folder accessible by user not the root of C:\) 
Even on your desktop, and run AgOpenGPS.exe

The software reads NMEA strings for the purpose of recording and mapping position information 
for Agricultural use. Also it has up to 16 sections of Section Control that can have unique widths 
or up to 64 same width sections to control implements application of product preventing 
over-application.

Also ouputs Pure pursuit steer angles from reference line for AB line, AB Curve and Contour guidance. 
Auto Headland called UTurn on Curve and AB Line with loops for narrow equipment. 
Mapping as a background can also be added.

Included in this repository is an application, and source folders. 

See the PCB repo for PCB layouts, firmware for steering and rate control, machine control, GPS and simulator. 

*** Important ****

If you distribute copies of such a program, whether
gratis or for a fee, you must pass on to the recipients the same
freedoms that you received.  You must make sure that they, too, receive
or can get the source code.  And you must show them these terms so they
know their rights as Outlined in the GPLv3 License.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

