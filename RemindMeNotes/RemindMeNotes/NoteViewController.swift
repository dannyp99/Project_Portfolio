//
//  NoteViewController.swift
//  RemindMeNotes
//
//  Created by Danny Pires on 5/4/20.
//  Copyright © 2020 Danny Pires. All rights reserved.
//

import UIKit

class NoteViewController: UIViewController {

    //Outlets for the two main components of the app the title and the note itself
    @IBOutlet var txtFieldTitle: UITextField!
    @IBOutlet var txtViewNote: UITextView!
    //used for the handling of the save
    var complete: ((String,String)->Void)?
    
    //Help with handling the passing of data back to the main view this is acceessible across the app.
    var noteTitle: String = ""
    var noteContent: String = ""
    
    override func viewDidLoad() {
        
        super.viewDidLoad()
        //Grab the fields for the note selected and loading this view.
        txtFieldTitle.text = noteTitle
        txtViewNote.text = noteContent
        txtFieldTitle.becomeFirstResponder()
        //Create a save button that when clicked will trigger selSave a objc function to save to the data.
        navigationItem.rightBarButtonItem = UIBarButtonItem(title: "Save", style: .done, target: self, action: #selector(selSave))
    }
    
    //Make sure the fields are filled if so then actually save through complete.
    @objc func selSave() {
        if let text = txtFieldTitle.text, !text.isEmpty, !txtViewNote.text.isEmpty {
            complete?(text, txtViewNote.text)
        }
    }
}
