//
//  ViewController.swift
//  RemindMeNotes
//
//  Created by Danny Pires on 5/4/20.
//  Copyright © 2020 Danny Pires. All rights reserved.
//
import CoreData
import UIKit

class ViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {
    // The table view to hold all of our notes
    @IBOutlet var table: UITableView!
    // These arrays serve to store the Core Data (Notes and Reminders)
    var noteItems: [NSManagedObject] = []
    var remItems: [NSManagedObject] = []
    //This is used for the note 'idx' which is stored in Core Date as an Int64
    var finalIndex: Int64 = 0
    // Used for the deletion of past reminders
    var timer = Timer()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        table.delegate = self
        table.dataSource = self
        title = "Notes"
        // This function will schedule itself to run every minute to call a function @objc to check for old reminders.
        timer = Timer.scheduledTimer(timeInterval: 60, target: self, selector: #selector(self.deletePastReminders), userInfo: nil, repeats: true)
    }
    
    //Called every minute
    @objc func deletePastReminders(){
        //Store the time that the function is called
        let now = Date()
        guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else {
            return
        }
        let modelContainer = appDelegate.persistentContainer.viewContext
        let fetchRequest = NSFetchRequest<NSFetchRequestResult>(entityName: "Reminders")
        //Query the Core Data for all reminders whos dates are before right now.
        fetchRequest.predicate = NSPredicate(format: "remDate < %@", now as NSDate)
        do {
            //Get the results
            guard let results = try modelContainer.fetch(fetchRequest) as? [NSManagedObject] else{
                return
            }
            //If any results are returned
            if results.count > 0 {
                //Delete them
                for reminder in results {
                    modelContainer.delete(reminder)
                }
            }else{
                //Let me know that no reminders were found.
                print("I got nothing")
            }
        } catch {
            print("Fetch Failed: \(error)")
        }

        do {
            //Save Core Data
            try modelContainer.save()
           }
        catch {
            print("Saving Core Data Failed: \(error)")
        }
    }
    
    override func viewWillAppear(_ animated: Bool){
        guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else{
            return
        }
            
        let modelContainer = appDelegate.persistentContainer.viewContext
            
        let getRequestNote = NSFetchRequest<NSManagedObject>(entityName: "Notes")
        let getRequestRem = NSFetchRequest<NSManagedObject>(entityName: "Reminders")
            
        do{
            //Set the Core Data arrays to grab all the Notes and Reminders
            noteItems = try modelContainer.fetch(getRequestNote)
            remItems = try modelContainer.fetch(getRequestRem)
            //If there are any notes get the 'idx' of the last element of the array. Array size is unreliable
            if noteItems.count > 0 {
                finalIndex = noteItems[noteItems.count - 1].value(forKey: "idx") as! Int64 + 1
            }
            print("Got all the things")
            //Check that my Reminders exist.
            for reminder in remItems{
                print(reminder.value(forKey: "remName")!)
            }
        }catch let error as NSError{
            print("Error getting data from entity managedObject. \(error), \(error.userInfo)")
        }
    }

    //Function to call the create Note view and call saveNote function to save note to Core Data
    @IBAction func addNote(){
        //Grab the specific view for creating a new view using the storyboard identifier
        guard let viewCon = storyboard?.instantiateViewController(identifier: "new") as? EntryViewController else {
            return
        }
        viewCon.title = "New Note"
        viewCon.navigationItem.largeTitleDisplayMode = .never
        //On completion (Pressing save as shown with the EntryViewController) we have these two strings that are passed
        viewCon.complete = { noteTitle, note in
            //First when we hit save, return the user to the "Notes" view
            self.navigationController?.popToRootViewController(animated: true)
            // Call save Note
            self.saveNote(noteTitle: noteTitle, note: note)
            //Reload the table with all the notes
            self.table.reloadData()
            
        }
        //Outside the complete we initially have to push the EntryViewController for the user to be able to make a note
        navigationController?.pushViewController(viewCon, animated: true)
    }
    
    //Save Note to Core Data
    func saveNote(noteTitle: String, note: String){
        guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else {
            return
        }
        let modelContainer = appDelegate.persistentContainer.viewContext
        let noteModel = NSEntityDescription.entity(forEntityName: "Notes", in: modelContainer)!
        // Create a noteItem to be added to Core Data
        let noteItem = NSManagedObject(entity: noteModel, insertInto: modelContainer)
        noteItem.setValue(noteTitle, forKey: "noteTitle")
        noteItem.setValue(note, forKey: "note")
        noteItem.setValue(finalIndex, forKey: "idx")
        
        do{
            //Attempt to save to Core Data and update the noteItems to have the new notes.
            try modelContainer.save()
            noteItems.append(noteItem)
        }catch let error as NSError{
            print("Failed to save noteItem: \(error), \(error.userInfo)")
        }
    }
    
    //Update the Note that is being edited in the Note view
    func updateNote(idx: Int64, noteTitle: String, note: String){
        guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else {
            return
        }
        let modelContainer = appDelegate.persistentContainer.viewContext
        let fetchRequest = NSFetchRequest<NSFetchRequestResult>(entityName: "Notes")
        //Query for the Note with the matching id
        fetchRequest.predicate = NSPredicate(format: "idx = %d", idx)
        do {
            //We should only ever get one result but we shouldn't just crash
            let results = try modelContainer.fetch(fetchRequest) as? [NSManagedObject]
            if results?.count != 0 {
                //So we only edit the first element which should be the only element
                results![0].setValue(noteTitle, forKey: "noteTitle")
                results![0].setValue(note, forKey: "note")
            }else{
                print("I got nothing")
            }
        } catch {
            print("Fetch Failed: \(error)")
        }

        do {
            try modelContainer.save()
           }
        catch {
            print("Saving Core Data Failed: \(error)")
        }
        
    }
    
    //Save a reminder to Core Data
    func saveReminder(remTitle: String, remDesc: String, remDate: Date){
        guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else {
            return
        }
        let modelContainer = appDelegate.persistentContainer.viewContext
        let remModel = NSEntityDescription.entity(forEntityName: "Reminders", in: modelContainer)!
        let remItem = NSManagedObject(entity: remModel, insertInto: modelContainer)
        //Create Core Data Item for reminder to be added.
        remItem.setValue(remTitle, forKey: "remName")
        remItem.setValue(remDesc, forKey: "remDesc")
        remItem.setValue(remDate, forKey: "remDate")
         do{
             try modelContainer.save()
             remItems.append(remItem)
            print("reminder saved!")
         }catch let error as NSError{
             print("Failed to save noteItem: \(error), \(error.userInfo)")
         }
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return noteItems.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        //Set the values for the Fields in each table cell.
        let cell = tableView.dequeueReusableCell(withIdentifier: "cell", for: indexPath)
        cell.textLabel?.text = noteItems[indexPath.row].value(forKey: "noteTitle") as? String
        cell.detailTextLabel?.text = noteItems[indexPath.row].value(forKey: "note") as? String
        return cell
    }
    func tableView(_ tableView: UITableView, editActionsForRowAt indexPath: IndexPath) -> [UITableViewRowAction]? {
        // Add the options for when you swipe a cell left.
        let delete = UITableViewRowAction(style: .destructive, title: "Delete") { (action, indexPath) in
            guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else {
                return
            }
            // When deleting option is selected delete the Note from Core Data
            let modelContainer = appDelegate.persistentContainer.viewContext
            modelContainer.delete(self.noteItems[indexPath.row])
            do {
                try modelContainer.save()
            }catch{
                print("Saving Core Data Failed: \(error)")
            }
            //Remove it from the array keeping track of notes to display and reload the table.
            self.noteItems.remove(at: indexPath.row)
            self.table.reloadData()
        }
        //Called share but this is for reminders
        let share = UITableViewRowAction(style: .normal, title: "Reminder") { (action, indexPath) in
            guard let viewCon = self.storyboard?.instantiateViewController(identifier: "reminder") as? ReminderViewController else{
                return
            }
            //Same idea as switching views, but doing it through the swipe options.
            viewCon.title = "Create Reminder"
            viewCon.navigationItem.largeTitleDisplayMode = .never
            self.navigationController?.pushViewController(viewCon, animated: true)
            // When Save is selected on the reminder.
            viewCon.completion = {title, desc, date in
                DispatchQueue.main.async {
                    self.navigationController?.popToRootViewController(animated: true)
                    self.saveReminder(remTitle: title, remDesc: desc, remDate: date)
                    UNUserNotificationCenter.current().requestAuthorization(options: [.alert, .badge, .sound], completionHandler:{success, error in
                        if success {
                            //Schedule the push notification
                            self.scheduleReminder(title,desc,date)
                        }else{
                            print("Error: \(error!)")
                        }
                    })
                }
            }
        }
        //editing the option colors
        share.backgroundColor = UIColor(red: 72/255, green: 133/255, blue: 237/255, alpha: 1.0)
        delete.backgroundColor = UIColor(red: 219/255, green: 50/255, blue: 54/255, alpha: 1.0)

        return [delete, share]
    }
    
    // Create the push notification to be sent at the time specified.
    func scheduleReminder(_ title: String, _ desc: String, _ date: Date){
        //Create the reminder object
        let content = UNMutableNotificationContent()
                        content.title = title
                        content.sound = .default
                        content.body = desc
        let remDate = date
        //Set the trigger, seconds not specified as we want it to go off on the minute user enters.
        let trigger = UNCalendarNotificationTrigger(dateMatching: Calendar.current.dateComponents([.year, .month, .day, .hour, .minute],
        from: remDate),
        repeats: false)
        //Request ability to send reminders
        let request = UNNotificationRequest(identifier: "some_long_id", content: content, trigger: trigger)
            UNUserNotificationCenter.current().add(request, withCompletionHandler: { error in
                if error != nil {
                    print("something went wrong")
                }
            })
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        //Setup to transfer to the Edit View
        let model = noteItems[indexPath.row]
        
        guard let viewCon = storyboard?.instantiateViewController(identifier: "note") as? NoteViewController else {
            return
        }
        //Works nearly the same as Entry when making a new note except we fill the fields with the selected note.
        viewCon.navigationItem.largeTitleDisplayMode = .never
        viewCon.noteTitle = model.value(forKey: "noteTitle") as! String
        viewCon.noteContent = model.value(forKey: "note") as! String
        let index = model.value(forKey: "idx") as! Int64
        viewCon.title = "Edit Note"
        navigationController?.pushViewController(viewCon, animated: true)
        viewCon.complete = { noteTitle, note in
            self.navigationController?.popToRootViewController(animated: true)
            self.updateNote(idx: index, noteTitle: noteTitle, note: note)
            self.table.reloadData()
        }
    }
}
