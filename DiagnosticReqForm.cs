using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Patient_Record
{
    public partial class DiagnosticReqForm : Form
    {
        private PrintDocument printDocument;

        public DiagnosticReqForm()
        {
            InitializeComponent();
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
        }

        private void button1Preview_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog
            {
                Document = printDocument
            };
            printPreviewDialog.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument
            };
            previewDialog.ShowDialog(); // Show print preview
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Set up fonts
            Font sectionFont = new Font("Arial", 12, FontStyle.Bold);
            Font testFont = new Font("Arial", 10, FontStyle.Regular);
            Font detailFont = new Font("Arial", 10, FontStyle.Regular);
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font requestTitleFont = new Font("Arial", 14, FontStyle.Bold);

            // Define margins
            float marginLeft = 70;
            float marginTop = 50;
            float marginRight = 50;
            float marginBottom = 50;

            // Calculate print area with margins
            RectangleF printArea = new RectangleF(
                marginLeft,
                marginTop,
                e.PageBounds.Width - marginLeft - marginRight,
                e.PageBounds.Height - marginTop - marginBottom
            );

            // Initialize variables for text drawing
            float currentY = marginTop;
            float logoWidth = 115;
            StringFormat centerFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Draw the logo on the left side
            Image logoImage = Image.FromFile("C:\\Users\\tala\\Downloads\\medical logo.jpg"); // Update the path to your logo image
            float logoHeight = (logoWidth / logoImage.Width) * logoImage.Height;
            e.Graphics.DrawImage(logoImage, marginLeft, currentY, logoWidth, logoHeight);

            // Draw the clinic details (title and address) next to the logo
            float clinicDetailsX = marginLeft + logoWidth + 10; // Adjust space between logo and details
            float clinicDetailsWidth = printArea.Width - logoWidth - 20; // Ensure details fit within the print area

            // Draw the "LCC1 MEDICAL AND DIAGNOSTIC CENTER" title
            string clinicTitle = "LCC1 MEDICAL AND DIAGNOSTIC CENTER";
            SizeF clinicTitleSize = e.Graphics.MeasureString(clinicTitle, titleFont, (int)clinicDetailsWidth, centerFormat);
            e.Graphics.DrawString(clinicTitle, titleFont, Brushes.Black, new RectangleF(clinicDetailsX, currentY, clinicDetailsWidth, clinicTitleSize.Height), centerFormat);
            currentY += clinicTitleSize.Height + 10;

            // Draw the rest of the clinic details centered below the title
            string[] clinicDetails = {
                "312 GALAS ST. BIGNAY VALENZUELA CITY",
                "DOH ACCREDITED LABORATORY with License #13-0679-19-CL-2",
                "Phone: 09566481052",
                "Email: staffclinic27@gmail.com"
            };
            foreach (string detail in clinicDetails)
            {
                SizeF detailSize = e.Graphics.MeasureString(detail, detailFont, (int)clinicDetailsWidth, centerFormat);
                e.Graphics.DrawString(detail, detailFont, Brushes.Black, new RectangleF(clinicDetailsX, currentY, clinicDetailsWidth, detailSize.Height), centerFormat);
                currentY += detailSize.Height;
            }

            // Draw patient's name, age, and date
            currentY += 20; // Add space before the next section
            e.Graphics.DrawString("Patient's Name: " + textBoxName.Text, detailFont, Brushes.Black, marginLeft, currentY);
            currentY += 20;
            e.Graphics.DrawString("Age: " + textBoxAge.Text, detailFont, Brushes.Black, marginLeft, currentY);
            currentY += 20;
            e.Graphics.DrawString("Date: " + dateTimePickerDate.Value.ToShortDateString(), detailFont, Brushes.Black, marginLeft, currentY);
            currentY += 40; // Add space before the next section

            // Draw the "Diagnostic Test Request" title centered
            string requestTitle = "Diagnostic Lab Request Form";
            SizeF requestTitleSize = e.Graphics.MeasureString(requestTitle, requestTitleFont);
            float requestTitleX = printArea.Left + (printArea.Width - requestTitleSize.Width) / 2;
            e.Graphics.DrawString(requestTitle, requestTitleFont, Brushes.Black, requestTitleX, currentY);
            currentY += requestTitleSize.Height + 30; // Add space before the next section

            // Set up column positions
            float leftColumnX = marginLeft;
            float rightColumnX = e.PageBounds.Width / 2 + 10; // Start the second column from the middle of the page

            // Draw the diagnostic sections in two columns
            float leftColumnY = currentY;
            float rightColumnY = currentY;

            // Draw Hematology and Blood Chemistry in the left column
            leftColumnY = DrawSectionWithCheckboxes(e, "HEMATOLOGY", new[]
            {
                new { Label = "CBC", Checkbox = checkBoxCBC },
                new { Label = "Platelet", Checkbox = checkBoxPlatelet },
                new { Label = "ABO Typing", Checkbox = checkBoxABO },
                new { Label = "RH Typing", Checkbox = checkBoxRHTyping }
            }, leftColumnX, leftColumnY, sectionFont, testFont);

            leftColumnY = DrawSectionWithCheckboxes(e, "BLOOD CHEMISTRY", new[]
            {
                new { Label = "FBS", Checkbox = checkBoxFBS },
                new { Label = "BUN", Checkbox = checkBoxBUN },
                new { Label = "BUA", Checkbox = checkBoxBUA },
                new { Label = "SGPT", Checkbox = checkBoxSGPT },
                new { Label = "SGOT", Checkbox = checkBoxSGOT },
                new { Label = "SODIUM", Checkbox = checkBoxSODIUM },
                new { Label = "POTASIUM", Checkbox = checkBoxPOTASIUM },
                new { Label = "CREATININE", Checkbox = checkBoxCREATININE },
                new { Label = "CHOLESTEROL", Checkbox = checkBoxCHOLESTEROL },
                new { Label = "TRIGLYCERIDES", Checkbox = checkBoxTRIGLYCERIDES },
                new { Label = "HDL/LDL/VLDL", Checkbox = checkBoxHDL },
                new { Label = "LIPID PROFILE", Checkbox = checkBoxLIPID },
                new { Label = "CHLORIDE", Checkbox = checkBoxCHOLORIDE },
                new { Label = "CALCIUM", Checkbox = checkBoxCALCIUM },
                new { Label = "HBA1C", Checkbox = checkBoxHBA1C }
            }, leftColumnX, leftColumnY, sectionFont, testFont);

            // Draw Urine, Ultrasound, Others, and Serology in the right column
            rightColumnY = DrawSectionWithCheckboxes(e, "URINE", new[]
            {
                new { Label = "Pregnancy Test", Checkbox = checkBoxPregnancyTest },
                new { Label = "Routine Urinalysis", Checkbox = checkBoxUrinalysis }
            }, rightColumnX, rightColumnY, sectionFont, testFont);

            rightColumnY = DrawSectionWithCheckboxes(e, "ULTRASOUND", new[]
            {
                new { Label = "Whole Abdomen", Checkbox = checkBoxWholeabdomen },
                new { Label = "Pelvic", Checkbox = checkBoxPelvic }
            }, rightColumnX, rightColumnY, sectionFont, testFont);

            rightColumnY = DrawSectionWithCheckboxes(e, "SEROLOGY", new[]
            {
                new { Label = "VDRL/RPR", Checkbox = checkBoxvdrl }
            }, rightColumnX, rightColumnY, sectionFont, testFont);

            rightColumnY = DrawSectionWithCheckboxes(e, "OTHERS", new[]
           {
                new { Label = "ECG", Checkbox = checkBoxECG },
                new { Label = "X-ray", Checkbox = checkBoxxray }
            }, rightColumnX, rightColumnY, sectionFont, testFont);

        }

        private float DrawSectionWithCheckboxes(PrintPageEventArgs e, string sectionTitle, dynamic[] items, float startX, float startY, Font sectionFont, Font testFont)
        {
            // Draw section title
            e.Graphics.DrawString(sectionTitle, sectionFont, Brushes.Black, startX, startY);
            startY += 30; // Add space between the section title and the first item

            foreach (var item in items)
            {
                // Draw the checkbox outline
                e.Graphics.DrawRectangle(Pens.Black, startX, startY, 15, 15); // Draw the checkbox outline

                if (item.Checkbox.Checked)
                {
                    // Draw checkmark
                    PointF[] checkmarkPoints = {
                        new PointF(startX + 3, startY + 7),  // Starting point of the checkmark
                        new PointF(startX + 7, startY + 12), // Middle point of the checkmark
                        new PointF(startX + 13, startY + 3)  // End point of the checkmark
                    };
                    e.Graphics.DrawLines(Pens.Black, checkmarkPoints); // Draw the checkmark
                }

                // Draw the text
                e.Graphics.DrawString(item.Label, testFont, Brushes.Black, startX + 20, startY); // Text to the right of the checkbox

                startY += 25; // Move to the next row, with added space
            }

            startY += 20; // Add space before the next section
            return startY;
        }
    }
}
