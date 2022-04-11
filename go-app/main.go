package main

import (
	"go-app/controllers"
	"fmt"
	"github.com/gorilla/mux"
	"github.com/prometheus/client_golang/prometheus/promhttp"
	log "github.com/sirupsen/logrus"
	"net/http"
	"os"
)

func init() {
	log.SetFormatter(&log.JSONFormatter{})
	log.SetOutput(os.Stdout)
	log.SetLevel(log.DebugLevel)
	log.SetReportCaller(true)
}

var LogPath = func(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		log.Info(fmt.Sprintf("%s: %s (%s)", r.Host, r.RequestURI, r.Method))
		next.ServeHTTP(w, r)
	})
}

func main() {
	router := mux.NewRouter()

	router.HandleFunc("/notes", controllers.NoteQuery).Methods(http.MethodGet, http.MethodOptions)
	router.HandleFunc("/notes", controllers.NoteCreate).Methods(http.MethodPost, http.MethodOptions)
	router.HandleFunc("/notes/{id}", controllers.NoteRetrieve).Methods(http.MethodGet, http.MethodOptions)
	router.HandleFunc("/notes/{id}", controllers.NoteUpdate).Methods(http.MethodPut, http.MethodOptions)
	router.HandleFunc("/notes/{id}", controllers.NoteDelete).Methods(http.MethodDelete, http.MethodOptions)

	router.Handle("/metrics", promhttp.Handler())

	router.Use(LogPath)

	log.Info("Listening on 8080")
	err := http.ListenAndServe(":8080", router)
	if err != nil {
		log.Fatal("ListenAndServe: ", err)
	}
}
