using Update_DB_Daily.Models;

namespace Update_DB_Daily.ServiceLayer
{
    //You should have IProjectRepository (then it can have add, update, delete) and a new class implementing it named ProjectDBRepository 
    // because lets say tomorrow you need to have to save these records to some file then you can simply implement IProjectRepository naming your ine new class as
    // ProjectFlatFileRepository

    //And from your code whereever you require to call any method call ProjectDBRepository
    public interface IProjectRepository
    {
        //use proper naming, it should be only InsertProjects
        public void AddProject(List<Project> projectList);

    }
}
