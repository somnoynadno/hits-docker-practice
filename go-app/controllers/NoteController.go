package controllers

import (
	"go-app/db"
	"go-app/models"
	u "go-app/utils"
	"encoding/json"
	"github.com/gorilla/mux"
	"github.com/jinzhu/gorm"
	"net/http"
)

var NoteCreate = func(w http.ResponseWriter, r *http.Request) {
	Note := &models.Note{}
	err := json.NewDecoder(r.Body).Decode(Note)

	if err != nil {
		u.HandleBadRequest(w, err)
		return
	}

	db := db.GetDB()
	err = db.Create(Note).Error

	if err != nil {
		u.HandleBadRequest(w, err)
	} else {
		res, _ := json.Marshal(Note)
		u.RespondJSON(w, res)
	}
}

var NoteRetrieve = func(w http.ResponseWriter, r *http.Request) {
	Note := &models.Note{}

	params := mux.Vars(r)
	id := params["id"]

	db := db.GetDB()
	err := db.First(&Note, id).Error

	if err != nil {
		if err == gorm.ErrRecordNotFound {
			u.HandleNotFound(w)
		} else {
			u.HandleBadRequest(w, err)
		}
		return
	}

	res, err := json.Marshal(Note)
	if err != nil {
		u.HandleBadRequest(w, err)
	} else if Note.ID == 0 {
		u.HandleNotFound(w)
	} else {
		u.RespondJSON(w, res)
	}
}

var NoteUpdate = func(w http.ResponseWriter, r *http.Request) {
	Note := &models.Note{}

	params := mux.Vars(r)
	id := params["id"]

	db := db.GetDB()
	err := db.First(&Note, id).Error

	if err != nil {
		if err == gorm.ErrRecordNotFound {
			u.HandleNotFound(w)
		} else {
			u.HandleBadRequest(w, err)
		}
		return
	}

	newNote := &models.Note{}
	err = json.NewDecoder(r.Body).Decode(newNote)

	if err != nil {
		u.HandleBadRequest(w, err)
		return
	}

	err = db.Model(&Note).Updates(newNote).Error

	if err != nil {
		u.HandleBadRequest(w, err)
	} else {
		u.Respond(w, u.Message(true, "OK"))
	}
}

var NoteDelete = func(w http.ResponseWriter, r *http.Request) {
	params := mux.Vars(r)
	id := params["id"]

	db := db.GetDB()
	err := db.Delete(&models.Note{}, id).Error

	if err != nil {
		u.HandleBadRequest(w, err)
	} else {
		u.Respond(w, u.Message(true, "OK"))
	}
}

var NoteQuery = func(w http.ResponseWriter, r *http.Request) {
	var notes []models.Note
	db := db.GetDB()

	query, ok := r.URL.Query()["query"]
	if !ok || len(query[0]) < 1 {
		err := db.Find(&notes).Error
		if err != nil {
			u.HandleBadRequest(w, err)
			return
		}
	} else {
		q := "%" + query[0] + "%"
		err := db.Where("title LIKE ? OR content LIKE ?", q, q).Find(&notes).Error
		if err != nil {
			u.HandleBadRequest(w, err)
			return
		}
	}

	res, err := json.Marshal(notes)
	if err != nil {
		u.HandleBadRequest(w, err)
	} else {
		u.RespondJSON(w, res)
	}
}
