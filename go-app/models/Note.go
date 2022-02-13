package models

import (
	"time"
)

type BaseModel struct {
	ID        uint       `gorm:"primary_key" json:"id"`
	CreatedAt time.Time  `json:"-"`
	UpdatedAt time.Time  `json:"-"`
	DeletedAt *time.Time `json:"-"`
}

type Note struct {
	BaseModel
	Title   string `json:"title"`
	Content string `json:"content"`
}
