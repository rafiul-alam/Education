package GUI;

import java.lang.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import Entities.*;

public class MyPanel extends JFrame implements MouseListener, ActionListener
{
	JPanel panel;
	JLabel name, pid,logoLabel;
	JButton searchbtn, regbtn, exitbtn;
	JTextField namefield, patientIdField;
	ImageIcon logo;

	Color clr, clr2, clr3;
	Font font1, font2;

	public MyPanel()
	{
		super("Patient Entry System");
		this.setSize(1200, 800);

		clr = new Color(40, 44, 52);
		clr2 = new Color(64, 120, 192);
		clr3 = new Color(120, 144, 156);

		font1 = new Font("Segoe UI", Font.BOLD, 17);
		font2 = new Font("Roboto", Font.BOLD, 18);

		panel = new JPanel();
		panel.setLayout(null);
		panel.setBackground(clr);

		logo = new ImageIcon("logo.jpg");
        logoLabel = new JLabel(logo);
        logoLabel.setBounds(435, 10,324, 103);
        panel.add(logoLabel);

		name = new JLabel("         Patient Name");
		name.setBounds(475, 150, 250, 50);
		name.setBackground(clr2);
		name.setOpaque(true);
		name.setFont(font1);
		name.setForeground(Color.WHITE);
		panel.add(name);

		namefield = new JTextField();
		namefield.setBounds(475, 210, 250, 40);
		panel.add(namefield);

		pid = new JLabel("         Patient ID");
		pid.setBounds(475, 270, 250, 50);
		pid.setFont(font2);
		pid.setOpaque(true);
		panel.add(pid);

		patientIdField = new JTextField();
		patientIdField.setBounds(475, 330, 250, 40);
		panel.add(patientIdField);

		searchbtn = new JButton("Search");
		searchbtn.setBounds(410, 400, 100, 50);
		searchbtn.setBackground(clr2);
		searchbtn.addMouseListener(this);
		searchbtn.addActionListener(this);
		panel.add(searchbtn);

		regbtn = new JButton("Add");
		regbtn.setBounds(530, 400, 100, 50);
		regbtn.setBackground(clr3);
		regbtn.addMouseListener(this);
		regbtn.addActionListener(this);
		panel.add(regbtn);

		exitbtn = new JButton("Exit");
		exitbtn.setBounds(650, 400, 100, 50);
		exitbtn.addMouseListener(this);
		exitbtn.addActionListener(this);
		panel.add(exitbtn);

		this.add(panel);
	}

	public void mouseClicked(MouseEvent me) {}
	public void mousePressed(MouseEvent me) {}
	public void mouseReleased(MouseEvent me) {}

	public void mouseEntered(MouseEvent me)
	{
		if (me.getSource() == searchbtn)
		{
			searchbtn.setBackground(Color.BLACK);
			searchbtn.setForeground(Color.WHITE);
		}
		else if (me.getSource() == regbtn)
		{
			regbtn.setBackground(Color.ORANGE);
			regbtn.setForeground(Color.WHITE);
		}
	}

	public void mouseExited(MouseEvent me)
	{
		if (me.getSource() == searchbtn)
		{
			searchbtn.setBackground(clr2);
			searchbtn.setForeground(Color.BLACK);
		}
		else if (me.getSource() == regbtn)
		{
			regbtn.setBackground(clr3);
			regbtn.setForeground(Color.BLACK);
		}
	}

	public void actionPerformed(ActionEvent ae)
	{
		String name = namefield.getText();
		String pid = patientIdField.getText();
		String command = ae.getActionCommand();

		if (ae.getSource() == searchbtn)
		{
			Patient ptnt = new Patient();

			if (ptnt.FindPatient(name, pid) == true)
			{
			    JOptionPane.showMessageDialog(null, "Patient Found");
			}
			else
			{
				JOptionPane.showMessageDialog(null, "Patient Not found");
			}
		}
		else if (ae.getSource() == regbtn)
		{
			Patient ptnt = new Patient(name, pid);
			ptnt.addpatient();
			namefield.setText("");
			patientIdField.setText("");
			JOptionPane.showMessageDialog(null, "Patient Data Added");
		}
		else if (exitbtn.getText().equals(command))
		{
			int choice = JOptionPane.showConfirmDialog(null, "Do you want to exit", "Exit bar", JOptionPane.YES_NO_OPTION);
			if (choice == JOptionPane.YES_OPTION)
			{
				System.exit(0);
			}
		}
	}
}
